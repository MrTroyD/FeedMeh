using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{

    [SerializeField]SoundPlayer _soundPlayer;
    [SerializeField]AudioSource _soundTrack;
    [SerializeField]float _hungerPerSecond = .5f;
    [SerializeField]Character _character;

    [SerializeField]Animator _animator;
    [SerializeField]Animator _plantAnimator;

    public enum PlantAttackMode
    {
        Idle,
        Attacking,
        Recoiling
    }

    [SerializeField]PlantAttackMode _currentBehaviour;

    [SerializeField]Transform _mouth;
    [SerializeField]List<FoodInArea> _foodInArea = new List<FoodInArea>();

    [SerializeField]float _hungerGauge = 10f;
    float _maxHungerArea = 5f;
    float _diffLevel = 1;
    float _diffIncreaseTimer = 30f;

    [SerializeField]float _tasteForMeat = 0;

    Vector3 _startingLocation;

    bool _attackReady = true;
    bool _sadSoundReady = true;

    Vector3 _attackPoint;
    float _attackSpeed = .25f;

    float _attackTime = 0f;

    public float healthBarPercentage;

    // Start is called before the first frame update
    void Start()
    {
        this._startingLocation = this._mouth.transform.position;
        this._currentBehaviour = PlantAttackMode.Idle;
        this._diffIncreaseTimer = 30;

        MainGame.Instance.soundManager.OnAddAudioSource(this._soundTrack);
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        PossibleFood pf = other.GetComponentInParent<PossibleFood>();
        if (pf && pf.lifeStatus != PossibleFood.FoodStatus.Inedible)
        {
            this._foodInArea.Add(new FoodInArea(pf));

            if (this._foodInArea.Count == 1)
            {
                this._soundPlayer.PlaySound("Squeek");
                this._animator.Play("FoodIndicator");
                this._mouth.LookAt(pf.transform);
                this._plantAnimator.Play("Idle");
            }
        }
    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerExit(Collider other) {
        PossibleFood pf = other.GetComponentInParent<PossibleFood>();
        if (pf)
        {
            FoodInArea fia;
            for(int i = this._foodInArea.Count -1; i >= 0; i--)
            {
                fia = this._foodInArea[i];
                if (pf == fia.food)
                {
                    this._foodInArea.RemoveAt(i);
                    continue;
                }
            }
        }  

        if (this._foodInArea.Count < 1)
        {
            this._animator.Play("FoodRemove");
        }      
    }

    public void OnEat(PossibleFood possibleFood)
    {
        this._hungerGauge += possibleFood.nurishValue;


        if (this._hungerGauge > 55) 
        {
            this._diffIncreaseTimer = 30;
            MainGame.Instance.OnLevelUp();
            this._hungerPerSecond = .5f + (MainGame.Instance.level * .2f);
            this._soundPlayer.PlaySound("Sing");
        }

        this._hungerGauge = Mathf.Clamp(this._hungerGauge, 0, 52);
        if (possibleFood.lifeStatus == PossibleFood.FoodStatus.Meat || possibleFood.lifeStatus == PossibleFood.FoodStatus.Alive)
        {
            this._tasteForMeat += .05f;

            if (possibleFood.lifeStatus == PossibleFood.FoodStatus.Alive)
            {
                this._tasteForMeat += .05f;
            }   

            this._soundPlayer.PlaySound("CrunchOne");
        }
        else
        {            
            this._soundPlayer.PlaySound("CrunchTwo");
            this._tasteForMeat -= .01f;
            if (this._tasteForMeat < 0) this._tasteForMeat = 0;
        }

        if (this._tasteForMeat > 1) this._tasteForMeat = 1;

        if (possibleFood.gameObject.tag == "Player")
        {
            this._soundTrack.Stop();
            MainGame.Instance.OnKillPlayer();
        }

        MainGame.Instance.OnFoodConsumed(possibleFood.gameObject);

        Destroy(possibleFood.gameObject, .01f);

        if (this._foodInArea.Count < 2)
        {
            this._animator.Play("FoodRemove");
        }  

    }

    // Update is called once per frame
    void Update()
    {
        if (!MainGame.Instance.gameActive) return;

        //If the player is within Range look at the player
        this._diffIncreaseTimer -= MainGame.Instance.deltaTime;
        this._hungerGauge -= MainGame.Instance.deltaTime * this._hungerPerSecond;
        if (this._sadSoundReady && this._hungerGauge < 2)
        {
            this._sadSoundReady = false;
            this._soundPlayer.PlaySound("Dying");
        }

        if (this._hungerGauge > 30)
        {
            this._sadSoundReady = true;
        }

        if (this._hungerGauge < 0) 
        {
            this._hungerGauge = 0;
            this.enabled = false;

            this._soundTrack.Stop();
            MainGame.Instance.GameOver();
        }

        this.healthBarPercentage = Mathf.Clamp(this._hungerGauge/(this._maxHungerArea * 10f), 0.001f, 1f);
        
        if (this._diffIncreaseTimer < 0)
        {
            this._diffIncreaseTimer = 30;
            MainGame.Instance.OnLevelUp();
        }

        for (int i = this._foodInArea.Count - 1; i >= 0; i--)
        {
            FoodInArea fia = this._foodInArea[i];
            if (fia.food == null)
            {
                this._foodInArea.RemoveAt(i);
                continue;   
            } 

            fia.timeInArea += (fia.food.lifeStatus == PossibleFood.FoodStatus.Veggies) ?  MainGame.Instance.deltaTime * (1-this._tasteForMeat) : MainGame.Instance.deltaTime;
            if (this._currentBehaviour != PlantAttackMode.Idle) continue;

            if (fia.timeInArea > this._hungerGauge || fia.timeInArea > this._maxHungerArea)
            {
                if (fia.food == null) continue;
                //TODO: Attack charge up
                this._currentBehaviour = PlantAttackMode.Attacking;

                this._plantAnimator.Play("Bite");
                this._attackPoint = fia.food.transform.position;
                this._attackTime = 0;
            }
        }

        if (this._currentBehaviour == PlantAttackMode.Attacking)
        {
            this._attackTime += MainGame.Instance.deltaTime;
            this._mouth.transform.position = Vector3.Lerp(this._startingLocation, this._attackPoint, this._attackTime/this._attackSpeed);
            this._mouth.LookAt(this._attackPoint);
            
            Collider[] hitColliders = Physics.OverlapSphere(this._mouth.position, .25f);
            if (hitColliders.Length > 0)
            {
                for (int n = 0; n < hitColliders.Length; n++)
                {
                    if (hitColliders[n].gameObject.layer == LayerMask.NameToLayer("Plant")) continue;
                
                PossibleFood pf = hitColliders[n].gameObject.GetComponentInParent<PossibleFood>();
                if (pf == null) continue;

                if (pf.lifeStatus == PossibleFood.FoodStatus.Veggies || pf.lifeStatus == PossibleFood.FoodStatus.Meat)
                    OnEat(pf);


        
                }
            }
            
            if (this._attackTime > this._attackSpeed)
            {
                this._attackTime = 0;
                this._currentBehaviour = PlantAttackMode.Recoiling;

               
            }
        }

        if (this._currentBehaviour == PlantAttackMode.Recoiling)
        {
            this._attackTime += MainGame.Instance.deltaTime;
            this._mouth.transform.position = Vector3.Lerp(this._attackPoint, this._startingLocation, this._attackTime/this._attackSpeed);

            if (this._attackTime > this._attackSpeed)
            {
                this._attackTime = 0;
                this._currentBehaviour = PlantAttackMode.Idle;
            }
        }

        

        switch (this._currentBehaviour)
        {
            case PlantAttackMode.Idle:
                OnIdling();
                break;
        }

    }
    void OnIdling()
    {   
        if (this._foodInArea.Count < 1)
        {
           // print (this._mouth.transform.rotation.y);
            //Slowly look up again if you're not hunting/biting
            this._mouth.transform.localRotation = Quaternion.RotateTowards(this._mouth.transform.transform.localRotation, Quaternion.Euler(15, this._mouth.transform.transform.localEulerAngles.y, 0), 1);
        }
        else
        {
            //Check what will be eaten next
            int minIndex = -1;
            float minTime = -1;

            for (int i = 0; i < this._foodInArea.Count; i++)
            {
                if (this._foodInArea[i].timeInArea > minTime)
                {
                    minTime = this._foodInArea[i].timeInArea;
                    minIndex = i;
                }
            }

            if (minIndex != -1) //It should never
            {
                this._mouth.LookAt(this._foodInArea[minIndex].food.transform);
            }
        }


    }

}


[System.Serializable]
class FoodInArea
{
    public PossibleFood food;
    public float timeInArea;

    public FoodInArea(PossibleFood possibleFood)
    {
        this.food = possibleFood;
        this.timeInArea = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantBehaviour : MonoBehaviour
{
    public enum PlantAttackMode
    {
        Idle,
        Attacking,
        Recoiling
    }

    [SerializeField]PlantAttackMode _currentBehaviour;

    [SerializeField]Transform _mouth;
    List<FoodInArea> _foodInArea = new List<FoodInArea>();

    [SerializeField]float _hungerGauge = 5f;

    Vector3 _startingLocation;

    bool _attackReady = true;

    Vector3 _attackPoint;
    float _attackSpeed = .25f;

    float _attackTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        this._startingLocation = this._mouth.transform.position;
        this._currentBehaviour = PlantAttackMode.Idle;
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        print ("Something is in my food area!");
        PossibleFood pf = other.GetComponentInParent<PossibleFood>();
        if (pf && pf.lifeStatus != PossibleFood.FoodStatus.Inedible)
        {
            this._foodInArea.Add(new FoodInArea(pf));
        }
    }

    void OnTriggerExit(Collider other) {
        PossibleFood pf = other.GetComponentInParent<PossibleFood>();
        print ("Something left my food area!"+this.name);
        if (pf)
        {
            FoodInArea fia;
            for(int i = this._foodInArea.Count -1; i >= 0; i--)
            {
                fia = this._foodInArea[i];
                if (pf == fia.food)
                {
                    print ("Removing that "+pf.name);
                    this._foodInArea.RemoveAt(i);
                    continue;
                }
            }
        }        
    }

    public void OnEat(PossibleFood possibleFood)
    {
        Destroy(possibleFood.gameObject, .01f);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = this._foodInArea.Count - 1; i >= 0; i--)
        {
            FoodInArea fia = this._foodInArea[i];
            if (fia.food == null)
            {
                this._foodInArea.RemoveAt(i);
                continue;   
            } 

            fia.timeInArea += Time.deltaTime;
            if (this._currentBehaviour != PlantAttackMode.Idle) continue;

            if (fia.timeInArea > this._hungerGauge)
            {
                if (fia.food == null) continue;
                //TODO: Attack charge up
                this._currentBehaviour = PlantAttackMode.Attacking;

                this._attackPoint = fia.food.transform.position;
                this._attackTime = 0;
            }
        }

        if (this._currentBehaviour == PlantAttackMode.Attacking)
        {
            this._attackTime += Time.deltaTime;
            this._mouth.transform.position = Vector3.Lerp(this._startingLocation, this._attackPoint, this._attackTime/this._attackSpeed);

            if (this._attackTime > this._attackSpeed)
            {
                this._attackTime = 0;
                this._currentBehaviour = PlantAttackMode.Recoiling;

                Collider[] hitColliders = Physics.OverlapSphere(this._mouth.position, .25f);
                 if (hitColliders.Length > 0)
                 {
                     for (int n = 0; n < hitColliders.Length; n++)
                     {
                         if (hitColliders[n].gameObject.layer == LayerMask.NameToLayer("Plant")) continue;
                        
                        PossibleFood pf = hitColliders[n].gameObject.GetComponentInParent<PossibleFood>();
                        if (pf == null) continue;

                        if (pf.lifeStatus == PossibleFood.FoodStatus.Veggies)
                            OnEat(pf);

                        print ("Something is in here named "+hitColliders[n].name);

                     }
                 }
            }
        }

        if (this._currentBehaviour == PlantAttackMode.Recoiling)
        {
            this._attackTime += Time.deltaTime;
            this._mouth.transform.position = Vector3.Lerp(this._attackPoint, this._startingLocation, this._attackTime/this._attackSpeed);

            if (this._attackTime > this._attackSpeed)
            {
                this._attackTime = 0;
                this._currentBehaviour = PlantAttackMode.Idle;
            }
        }
    }
}

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

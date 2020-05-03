using UnityEngine;
using System.Collections.Generic;


public class FoodSpawner : MonoBehaviour
{
    [SerializeField]GameObject _veggiesPrefab;
    [SerializeField]GameObject _humanPrefab;
    [SerializeField]GameObject _meatPrefab; //TODO

    [SerializeField]Transform[] _humanSpawnPoint;
    [SerializeField]Transform[] _foodSpawnPoint;

    int _initialMax = 5;

    List<GameObject> _activeConsumables;
    float _spawnTimer;

    public List<GameObject> activeConsumables
    {
        get {return this._activeConsumables;}
    }

    void Awake()
    {
        ClearList();
    }

    public void ClearList()
    {
        if (this._activeConsumables != null)
        {
            for (int i = this._activeConsumables.Count - 1; i >=0; i--)
            {
                if (this._activeConsumables[i] != null) Destroy(this._activeConsumables[i]);
            }
        }

        this._activeConsumables = new List<GameObject>();
        this._spawnTimer = 0;
    }

    public void SpawnInitial()
    {       

        for (int i = 0; i < 5; i++)
        {
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            GameObject foodObject = Instantiate(this._veggiesPrefab, new Vector3(Mathf.Cos(angle) * (i + 3.5f), -0.1f, Mathf.Sin(angle) * (i + 1.5f)), Quaternion.identity);
            OnSpawnFoodItem(foodObject);
        }

        for (int i = 0; i < 3; i++)
        {
            float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
            GameObject foodObject = Instantiate(this._humanPrefab, new Vector3(Mathf.Cos(angle) * (i + 3.5f), -0.1f, Mathf.Sin(angle) * (i + 1.5f)), Quaternion.identity);
            OnSpawnFoodItem(foodObject);
        }
    }

    void Update()
    {
        //Every 5 seconds make sure there is food for the plant
        if (MainGame.Instance.gamePaused || !MainGame.Instance.gameActive) return;

        this._spawnTimer += MainGame.Instance.deltaTime;
        if (this._spawnTimer >= 5)
        {
            print ("Spawning new");
            this._spawnTimer = 0;
            UpdateSpawn();
        }
    }

    public void OnSpawnFoodItem(GameObject foodObject)
    {
        this._activeConsumables.Add(foodObject);
        MainGame.Instance.soundManager.OnAddAudioSource(foodObject.GetComponent<AudioSource>());
    }
    

    public void OnRemoveFoodItem(GameObject foodObject)
    {
        if (this._activeConsumables.IndexOf(foodObject) != -1) this._activeConsumables.Remove(foodObject);
    }

    public void UpdateSpawn()
    {
        if (this._activeConsumables.Count > MainGame.Instance.level + this._initialMax) return;

        Vector3 playerLocation = MainGame.Instance.playerObject.transform.position;

        //spawn a piece of food offscreen
        //Always make sure there's at least one human on board
        bool humanExists = false;
        foreach(GameObject go in this._activeConsumables)
        {
            PossibleFood pf = go.GetComponent<PossibleFood>();
            if (pf.lifeStatus == PossibleFood.FoodStatus.Alive)
            {
                humanExists = true;
                break;
            }
        }

        for (int i = 0; i < MainGame.Instance.level + this._initialMax; i++)
        {
            List<Vector3> positions = new List<Vector3>();

            int n = 0;
            if (!humanExists)
            {
                
                for (n = 0; n < this._humanSpawnPoint.Length; n++)
                {
                    if (Vector3.Distance(playerLocation, this._humanSpawnPoint[n].position) > 3)
                    {
                        positions.Add(this._humanSpawnPoint[n].position);
                    }
                }

                GameObject newHumanFood = Instantiate(this._humanPrefab, this._humanSpawnPoint[Random.Range(0, this._humanSpawnPoint.Length)].position, Quaternion.identity);

                //Pick one of the 4 entrences and have the human spawn there
                humanExists = true;

                OnSpawnFoodItem(newHumanFood);
                continue;
            }

            for (n = 0; n < this._foodSpawnPoint.Length; n++)
            {
                if (Vector3.Distance(playerLocation, this._foodSpawnPoint[n].position) > 6)
                {
                    positions.Add(this._foodSpawnPoint[n].position);
                }
            }

            GameObject newFoodFood = Instantiate(((Random.Range(0f, 1f) > .75f) ? this._humanPrefab: this._veggiesPrefab), positions[Random.Range(0, positions.Count)], Quaternion.identity );   
            
            OnSpawnFoodItem(newFoodFood);
        }
    }

}

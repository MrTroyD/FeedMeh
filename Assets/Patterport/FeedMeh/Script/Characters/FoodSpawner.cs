using UnityEngine;
using System.Collections.Generic;


public class FoodSpawner : MonoBehaviour
{
    [SerializeField]GameObject _veggiesPrefab;
    [SerializeField]GameObject _humanPrefab;
    [SerializeField]GameObject _meatPrefab; //TODO

    List<GameObject> _activeConsumables;

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

    public void OnSpawnFoodItem(GameObject foodObject)
    {
        this._activeConsumables.Add(foodObject);
        MainGame.Instance.soundManager.OnAddAudioSource(foodObject.GetComponent<AudioSource>());
    }
    

    public void OnRemoveFoodItem(GameObject foodObject)
    {
        if (this._activeConsumables.IndexOf(foodObject) != -1) this._activeConsumables.Remove(foodObject);
        MainGame.Instance.soundManager.RemoveAudioSource(foodObject.GetComponent<AudioSource>());
    }

}

using UnityEngine;

public class ChooseOneRemoveOthers : MonoBehaviour
{
    [SerializeField]GameObject[] objects;
    // Start is called before the first frame update
    void Awake()
    {
        int ind = Random.Range(0, objects.Length);

        for (int i= 0; i < objects.Length; i++)
        {
            if (ind == i) continue;

            Destroy(objects[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

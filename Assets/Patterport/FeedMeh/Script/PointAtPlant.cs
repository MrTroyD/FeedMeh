using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtPlant : MonoBehaviour
{
    [SerializeField]Transform _target;
    
    // Update is called once per frame
    void Update()
    {
        if (this._target)
        {
            //I'm sorry! I need a better/cleaner way to do this
            Vector3 des = new Vector3(this._target.position.x, this.transform.position.y, this._target.position.z);
            this.transform.LookAt(this._target.position, transform.up);
        }   
    }

    public void SetTarget(Transform newTarget)
    {
        this._target = newTarget;
    }
}

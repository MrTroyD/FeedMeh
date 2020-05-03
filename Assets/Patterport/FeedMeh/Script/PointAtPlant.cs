using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtPlant : MonoBehaviour
{
    [SerializeField]Transform _target;

    [SerializeField]Renderer _renderComp;

    [SerializeField]MeshRenderer _rm1;
    [SerializeField]MeshRenderer _rm2;
    
    // Update is called once per frame
    void Update()
    {
        if (this._target)
        {
            //I'm sorry! I need a better/cleaner way to do this
            Vector3 des = new Vector3(this._target.position.x, this.transform.position.y, this._target.position.z);
            this.transform.LookAt(des, transform.up);

            if (this._renderComp)
            {
                this._rm1.enabled = !this._renderComp.isVisible;
                this._rm2.enabled = !this._renderComp.isVisible;
                
            }
        
        }   
    }

    public void SetTarget(Transform newTarget)
    {
        this._target = newTarget;
        this._renderComp = newTarget.GetComponentInChildren<Renderer>();
    }
}

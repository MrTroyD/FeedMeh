using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    public Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
        this.cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
       this.transform.LookAt(cam.transform.rotation * Vector3.back + transform.position, cam.transform.rotation * Vector3.up);
    }
}

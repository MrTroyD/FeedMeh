using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera : MonoBehaviour
{
    [SerializeField] Vector3 _offset;
    [SerializeField] Transform _target;
    [SerializeField] float _smoothSpeed = .015f;

    [SerializeField]float _clampH = 16;
    [SerializeField]float _clampTop = 7.5f;
    [SerializeField]float _clampBottom = -30f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (_target == null) return;

        Vector3 desiredPosition = _target.position + this._offset; 

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, -this._clampH, this._clampH);
        desiredPosition.z = Mathf.Clamp(desiredPosition.z, this._clampBottom, this._clampTop);
        Vector3 smoothPosition = Vector3.Lerp(this.transform.position, desiredPosition, this._smoothSpeed);

        this.transform.position = smoothPosition;

    }

    public void OnSetTo(Transform destination)
    {
        this._target = destination;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
    public Camera cam;
    private float _duration = 0;
    [SerializeField]TMPro.TMP_Text _text;

    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        this.cam = Camera.main;
    }

    public void OnSay(string message)
    {
        OnSay(message, 5f);
    }

    public void OnSay(string message, float duration)
    {
        this._text.text = message;
        this._duration = duration;
    }

    void Update()
    {
        if(this._duration > 0)
        {
            this._duration -= Time.deltaTime;
            if (this._duration < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (this.target) this.transform.position = this.target.position + (Vector3.up * .48f);
        this.transform.LookAt(cam.transform.rotation * Vector3.back + transform.position, cam.transform.rotation * Vector3.up);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeechManager : MonoBehaviour
{
    [SerializeField]SpeechBubble _speechPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSpeech(Transform destination, string message, float duration)
    {
        SpeechBubble newBubble = Instantiate(this._speechPrefab, destination.position, Quaternion.identity);
        newBubble.OnSay(message, duration);
        newBubble.target = destination;
    }
}

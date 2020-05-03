using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    [SerializeField]SoundLibrary[] clips;
    [SerializeField]AudioSource _audioSource;
    // Update is called once per frame
    public void PlaySound(string nom)
    {
        foreach(SoundLibrary clip in clips)
        {
            if (clip.name == nom)
            {
                this._audioSource.PlayOneShot(clip.clip, .75f);
                return;
            }
        }

        //Sound not found
    }
}

[System.Serializable]
public class SoundLibrary
{
    public string name;
    public AudioClip clip;
}

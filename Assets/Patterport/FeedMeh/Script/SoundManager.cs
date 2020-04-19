using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField]List<AudioSource> _audioSources;

    [SerializeField]float _volume;

    public float volume
    {
        get {return this._volume;}
    }

    // Start is called before the first frame update
    void Start()
    {
       ClearAudioSources();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnAddAudioSource(AudioSource audioSource)
    {
        if (this._audioSources.IndexOf(audioSource) != -1) return;
        this._audioSources.Add(audioSource);

        audioSource.volume = this._volume;

    }

    public void RemoveAudioSource(AudioSource audioSource)
    {
        if (this._audioSources.IndexOf(audioSource) == -1) return;
        this._audioSources.Remove(audioSource);

    }

    public void OnAudioChange(float newVolume)
    {
        if (this._volume == newVolume) return;
        this._volume = newVolume;
        for (int i = this._audioSources.Count -1; i >=0; i--)
        {
            if (this._audioSources[i] == null) 
            {
                this._audioSources.RemoveAt(i);
                continue;
            }

            this._audioSources[i].volume = newVolume;
        }

    }

    public void ClearAudioSources()
    {
        this._audioSources = new List<AudioSource>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public GameObject _quitButton;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        #if !UNITY_STANDALONE
            this._quitButton.SetActive(false);
        #endif
    }   

    public void OnReEnable()
    {
        MainGame.Instance.soundManager.OnAddAudioSource(GetComponent<AudioSource>());        
    }

    public void OnGameStart()
    {
        this.gameObject.SetActive(false);
        MainGame.Instance.OnResetGame();
    }

    public void OnQuit()
    {
        Application.Quit();
    }

}

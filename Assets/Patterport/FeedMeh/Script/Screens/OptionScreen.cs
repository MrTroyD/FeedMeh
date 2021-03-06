﻿using UnityEngine;
using UnityEngine.UI;

public class OptionScreen : MonoBehaviour
{
    [SerializeField]Image _backgroundImage;
    [SerializeField]GameObject _titleScreen;
    [SerializeField]Slider _volumeSlider;

    public void OnBack()
    {
        this.gameObject.SetActive(false);
        this._titleScreen.SetActive(true);
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    void OnEnable()
    {
        if ( MainGame.Instance == null) return;
        this._volumeSlider.value = MainGame.Instance.soundManager.volume;
    }

    void Update()
    {        
        if (this._backgroundImage)
        {
            this._backgroundImage.material.SetTextureOffset("_MainTex", new Vector2(Time.unscaledTime * .015f, Time.unscaledTime * .015f));
        }
    }
}

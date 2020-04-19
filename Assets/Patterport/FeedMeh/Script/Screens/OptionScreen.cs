using UnityEngine;
using UnityEngine.UI;

public class OptionScreen : MonoBehaviour
{
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
}

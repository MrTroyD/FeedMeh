using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{


    PlantBehaviour _plant;

    [SerializeField]Image _plantHunger;

    [SerializeField]TMP_Text _levelDisplay;
    [SerializeField]TMP_Text _score;
    [SerializeField]TMP_Text _time;

    [SerializeField]Animator _animator;

    [SerializeField]TMP_Text _toolTip;

    [SerializeField]GameObject _gameOver;

    float _tipTimer;

    void Update()
    {
        if (!this._plant || !MainGame.Instance.gameActive || MainGame.Instance.gamePaused) return;

        this._plantHunger.transform.localScale = new Vector3( this._plant.healthBarPercentage, 1, 1);

        UpdateTime();

        if (this._tipTimer > 0)
        {
            this._tipTimer -= Time.deltaTime;

            if (this._tipTimer <= 0)
            {
                this._toolTip.text = "";
            }
        }

    }

    public void SetUpPlant(PlantBehaviour plant)
    {
        this._plant = plant;
    }

    public void OnSetLevel()
    {
        float level = MainGame.Instance.level;
        this._levelDisplay.text = level > 1 ? "x"+level : "";
        this._animator.Play("UI_StartGame");

        ShowTip("Press Arrows to move");
    }

    public void UpdateScore()
    {
        this._score.text = MainGame.Instance.score.ToString("00000");
    }

    public void UpdateTime()
    {
        int seconds = Mathf.FloorToInt(MainGame.Instance.gameTime) % 60;
        int minutes = Mathf.FloorToInt(MainGame.Instance.gameTime / 60);
        this._time.text = minutes.ToString("00") + ":"+seconds.ToString("00");
    }

    public void ShowTip(string message)
    {
        ShowTip(message, 4);
    }

    public void ShowGameOver()
    {
        this._gameOver.SetActive(true);
    }

    public void HideGameOver()
    {
        this._gameOver.SetActive(false);
    }

    public void ShowTip(string message, float duration)
    {
        this._toolTip.text = message;
        this._tipTimer = duration;
    }
}

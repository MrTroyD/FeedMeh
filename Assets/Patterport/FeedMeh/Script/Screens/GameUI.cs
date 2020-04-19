﻿using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameUI : MonoBehaviour
{
    PlantBehaviour _plant;

    [SerializeField]Image _plantHunger;

    [SerializeField]TMP_Text _levelDisplay;
    [SerializeField]TMP_Text _score;

    void Update()
    {
        if (!this._plant || !MainGame.Instance.gameActive || MainGame.Instance.gamePaused) return;

        this._plantHunger.transform.localScale = new Vector3(1, this._plant.healthBarPercentage, 1);

    }

    public void SetUpPlant(PlantBehaviour plant)
    {
        this._plant = plant;
    }

    public void OnSetLevel()
    {
        float level = MainGame.Instance.level;
        this._levelDisplay.text = level > 1 ? "x"+level : "";
    }

    public void UpdateScore()
    {
        this._score.text = MainGame.Instance.score.ToString("00000");
    }
}
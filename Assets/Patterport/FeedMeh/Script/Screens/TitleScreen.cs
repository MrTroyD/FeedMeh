using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{

    public void OnGameStart()
    {
        this.gameObject.SetActive(false);
        MainGame.Instance.OnResetGame();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainGame : MonoBehaviour
{

    public static MainGame Instance
    {
        get {return _instance;}
    }

    static MainGame _instance;

    [SerializeField]SoundManager _soundManager;
    [SerializeField]TitleScreen _titleScreen;
    [SerializeField]OptionScreen _optionsScreen;
    bool _gameActive = false;

    [SerializeField]GameObject _playerPrefab;
    [SerializeField]GameObject _plantPrefab;

    [SerializeField]SmoothCamera _smoothCamera;

    [SerializeField]GameUI _gameUI;

    GameObject _plantObject;
    GameObject _playerObject;

    [SerializeField]SpeechManager _speechMananger;

    int _score;
    float _scoreTimer;
    float _gameTime;
    [SerializeField]FoodSpawner _foodSpawner;

    int _level = 1;

    public FoodSpawner foodSpawner
    {
        get {return this._foodSpawner;}
    }

    public SoundManager soundManager
    {
        get {return this._soundManager;}
    }

    public int level
    {
        get {return this._level;}
    }

    public float gameTime
    {
        get {return this._gameTime;}
    }

    public bool gameActive
    {
        get {return this._gameActive;}
    }

    public bool gamePaused
    {
        get {return Time.timeScale == 0;}
    }

    public int score
    {
        get {return this._score;}
    }

    public GameObject playerObject
    {
        get {return this._playerObject;}
    }

    public SpeechManager speechManager
    {
        get {return this._speechMananger;}
    }

    // Start is called before the first frame update
    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;    
        this._gameUI.HideGameOver();
    }

    private void OnEnable() {
        _instance = this;
    }

    public void OnFoodConsumed(GameObject possibleFood)
    {
        this._foodSpawner.OnRemoveFoodItem(possibleFood);
        MainGame.Instance.soundManager.RemoveAudioSource(possibleFood.GetComponent<AudioSource>());
    
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.gameActive || this.gamePaused) return;
        this._scoreTimer += Time.deltaTime;
        this._gameTime += Time.deltaTime;

        if(this._scoreTimer > 1)
        {
            this._scoreTimer -= 1;
            this._score += 250 * this._level;
            this._gameUI.UpdateScore();
        }

        //Debug
        if (Input.GetKeyUp(KeyCode.Q))
        {
            this._speechMananger.OnSpeech(this._playerObject.transform, "*ack*", 4);
        }
    }

    public void OnResetGame()
    {
        this._smoothCamera.OnSetTo(null);
        //Clear Game
        ClearGame();
        
        SetupGame();
    }

    public void ShowTip(string message, float duration)
    {
        this._gameUI.ShowTip(message, duration);
    }

    void SetupGame()
    {
        this._scoreTimer = 0;
        this._gameTime = 0;
        this._score = 0;
        this._level = 1;
        this._gameUI.OnSetLevel();
        this._gameUI.UpdateScore();

        this._gameActive = true;
        //Setup Plant
        this._plantObject = Instantiate(this._plantPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        this._soundManager.OnAddAudioSource(this._plantObject.GetComponent<AudioSource>());
        
        //Setup Player
        this._playerObject = Instantiate(this._playerPrefab, new Vector3(2, 0, 0), Quaternion.identity);
        this._soundManager.OnAddAudioSource(this._plantObject.GetComponent<AudioSource>());
        
        this._foodSpawner.SpawnInitial();

        this._smoothCamera.OnSetTo(this._playerObject.transform);
        this._playerObject.GetComponentInChildren<PointAtPlant>().SetTarget(this._plantObject.transform);
        this._gameUI.SetUpPlant(this._plantObject.GetComponent<PlantBehaviour>());
    }


    public void OnKillPlayer()
    {
        this._playerObject = null;

        GameOver();
    }

    public void GameOver()
    {
        print ("Game Over");
        this._gameActive = false;

        this._gameUI.ShowGameOver();

        Invoke("ShowTitle", 3f);
    }

    public void OnLevelUp()
    {
        print ("Level Up!");
        this._level++;
        this._gameUI.OnSetLevel();
    }


    void ShowTitle()
    {        
        this._gameUI.HideGameOver();
        this._titleScreen.gameObject.SetActive(true);
    }

    public void OnOptions()
    {
        if (this._optionsScreen.isActiveAndEnabled)
        {
            OnOptionsComplete();
            return;
        }
        if (this._gameActive)
        {
            print ("Pause Game");
            Time.timeScale = 0;
        }
        else
        {
            // this._titleScreen.gameObject.SetActive(false);
        }
        
        this._optionsScreen.gameObject.SetActive(true);
    }

    public void OnOptionsComplete()
    {
        if (this._gameActive)
        {
            print ("Unpause Game");
            Time.timeScale = 1;
        }
        
        this._optionsScreen.gameObject.SetActive(false);
    }


    void ClearGame()
    {
        this._soundManager.ClearAudioSources();
        this._gameUI.SetUpPlant(null);
    
        this._gameActive = false;
        if (this._plantObject) Destroy(this._plantObject);

        if (this._playerObject) Destroy(this._playerObject);

        this._foodSpawner.ClearList();
       
        this._plantObject = null;
        this._playerObject = null;

        
        this._level = 1;
        this._gameUI.OnSetLevel();
    }
}

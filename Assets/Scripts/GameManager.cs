using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public GameSound gs;

    CanvasGroup cg;
    AudioSource ads;

    public AudioClip StartScene;
    public AudioClip Level1;
    public AudioClip Level2;
    public AudioClip Level3;
    public AudioClip Credits;

    List<AudioClip> bgms;

    public static int currentLevel = 1;
    public static int highestLevel = 1;

    [SerializeField] private TextMeshProUGUI _gameTimer;
    [SerializeField] private TextMeshProUGUI _gameScore;
    [SerializeField] private TextMeshProUGUI _gameHelper;
    [SerializeField] private TextMeshProUGUI _gameDeathTimer;
    [SerializeField] private TextMeshProUGUI _gameDeathHelper;

    private float timer = 0f;
    private float maxDeathTime = 6f;
    public float maxTime = 1000.0f;
    private float loadingTimer = 0f;
    private float loadingTime = 1f;
    public bool isLoading = true;



    int coins = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        cg = this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<CanvasGroup>();
        gs = GetComponent<GameSound>();
        ads = GetComponent<AudioSource>();
        bgms = new List<AudioClip>();
        bgms.Add(StartScene);
        bgms.Add(Level1);
        bgms.Add(Level2);
        bgms.Add(Level3);
        bgms.Add(Credits);
    }

    void Update()
    {
        updateTimer();
        updateScore();
        updateHelper();
        updateLoading();
        updateDeathTimer();
    }

    void updateTimer()
    {
        timer += Time.deltaTime;
        _gameTimer.text = "Elapsed Time: " + System.Math.Round(timer, 3);
    }
    void updateLoading() {
        loadingTimer += Time.deltaTime;
        if (isLoading)
        {
            if (loadingTimer > loadingTime)
            {
                isLoading = false;
                loadingTimer = 0f;
                cg.alpha = 0f;
                return;
            }
            cg.alpha = 1f - loadingTimer / loadingTime;
        }
        
    }

    void updateScore()
    {
        _gameScore.text = "Score: " + coins;
    }

    void updateHelper()
    {
        if (PlayerManager.instance.PlayerCanPushBox() && !PlayerManager.instance.PlayerHasPushBox())
        {
            _gameHelper.text = "Hint: If you come in contact with boxes, press the 'E' key to push the box.";
        }
        else if (PlayerManager.instance.PlayerCanPushButton() && !PlayerManager.instance.PlayerHasPushButton())
        {
            _gameHelper.text = "Hint: If you come in contact with a door, press the 'E' key to open the door.";
        }
        else
        {
            _gameHelper.text = "";
        }

    }

    void updateDeathTimer(){
        if(PlayerManager.instance.PlayerHasDied()){
            maxDeathTime -= Time.deltaTime;
            _gameDeathHelper.text = "YOU HAVE DIED.";
            _gameScore.text = "";
            coins = 0;
            timer = 0;
            _gameTimer.text = "";
            _gameDeathTimer.text = "Respawining in " + System.Math.Round(maxDeathTime, 0) + " seconds. Or Exit with 'ESC' key.";
        }
        else{
            _gameDeathHelper.text = "";
            _gameDeathTimer.text = "";
            maxDeathTime = 6f;
        }
    }

    public void LoadLevel(string name = "default")
    {
        if (name == "default") name = "Level" + currentLevel;
        SceneManager.LoadScene(name);
        if (name == "Credits" || name == "StartScene") {
            SetBackgroundMusic(4);
            return;
        }
        else {
            currentLevel = int.Parse(name.Substring(5));
            SetBackgroundMusic(currentLevel);
        }
        isLoading = true;
        timer = 0f;
        loadingTimer = 0f;
        PlayerManager.instance.SetPosition(new Vector3(0, 1, 0));
    }

    public void CollectCoin()
    {
        coins += 1;
        gs.CollectCoin();
    }

    void SetBackgroundMusic(int index) {
        ads.Pause();
        ads.clip = bgms[index];
        ads.Play(0);
    }
}

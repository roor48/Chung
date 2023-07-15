using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance ? _instance : null;

    private Camera mainCam;
    public Camera MainCam
    {
        get
        {
            if (!mainCam)
                mainCam = Camera.main;
            return mainCam;
        }
    }

    public float Timer { get; private set; }
    public int Score { get; set; }

    public bool isCleared = false;

    private void Awake()
    {
        if (_instance)
        {
            Destroy(this.gameObject);
        }
        else
            _instance = this;
    
        DontDestroyOnLoad(this.gameObject);
    }
    private void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Timer = 0;
        Score = 0;
        SetCleared(false);
    }
    private void Update()
    {
        if (isCleared)
            return;
        Timer += Time.deltaTime;
    }

    public void SetCleared(bool _flag)
    {
        isCleared = _flag;
    }

    public void NextScene(int sceneNum)
    {
        SceneManager.LoadScene("Stage " + sceneNum);
    }
}

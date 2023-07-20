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

    // 데이터 합계
    public float sumTime;
    public int sumTakenDmg;
    public int sumGivenDmg;
    public int sumScore;

    // 현재 정보
    public float Timer { get; private set; }
    public int Score { get; set; }
    public int TakenDmg { get; set; }
    public int GivenDmg { get; set; }

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
        // 합을 구하고
        sumTime += Timer;
        sumTakenDmg += TakenDmg;
        sumGivenDmg += GivenDmg;
        sumScore += Score;
        
        // 다음 씬으로 넘어가면 초기화한다.
        TakenDmg = 0;
        GivenDmg = 0;
        Timer = 0;
        Score = 0;
        SetCleared(false);
    }
    private void Update()
    {
        if (isCleared)
            return;
        Timer += Time.deltaTime; // 시간 추가
    }

    public void SetCleared(bool _flag)
    {
        isCleared = _flag;
    }
}

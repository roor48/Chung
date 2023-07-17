using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Animator stageStart;
    [SerializeField] private Animator stageClear;
    
    [Header("InPlay UI")]
    public TMP_Text healthText;
    public TMP_Text timeText;
    public TMP_Text scoreText;
    public TMP_Text levelText;
    public Slider burstSlider;
    public Animator levelUpAnim;
    public Slider expSlider;
    public GameObject pausePanel;
    
    [Header("GameOver UI")]
    public GameObject goPanel;
    private Text gameOverTimeText;
    private Text givenDmgText;
    private Text takenDmgText;
    private Text gameOverLevelText;
    
    [Header("ClearPanel UI")]
    public GameObject clearPanel;
    public TMP_Text panelScore;
    public TMP_Text panelTime;
    public TMP_Text clearGivenDmg;
    public TMP_Text clearTakenDmg;

    [Header("Cheat UI")]
    public GameObject cheatPanel;
    public TMP_Text cPText;

    public TakeDamage playerHealth;

    private void Awake()
    {
        stageStart.SetTrigger(AnimatorID.showText);

        gameOverTimeText = goPanel.transform.GetChild(0).GetComponent<Text>();
        givenDmgText = goPanel.transform.GetChild(1).GetComponent<Text>();
        takenDmgText = goPanel.transform.GetChild(2).GetComponent<Text>();
        gameOverLevelText = goPanel.transform.GetChild(3).GetComponent<Text>();
    }

    private void Update()
    {
        healthText.text = $"남은 <color=#FF0000>체력</color> : <b>{playerHealth.health}</b>";
        int time = (int)GameManager.Instance.Timer;
        timeText.text = $"{time / 60:D2} : {time % 60:D2}";
        scoreText.text = $"{GameManager.Instance.Score:#,##0}점";
        levelText.text = $"현재 레벨 : {PlayerMove.Instance.level}";

        panelScore.text = $"점수 : {GameManager.Instance.Score:#,##0}";
        panelTime.text = $"걸린 시간 : {time / 60:D2}분 {time % 60:D2}초";
        clearGivenDmg.text = $"가한 피해량 : {GameManager.Instance.GivenDmg:#,##0}";
        clearTakenDmg.text = $"받은 피해량 : {GameManager.Instance.TakenDmg:#,##0}";

        burstSlider.value = 1 - PlayerMove.Instance.curBurstGauge / PlayerMove.Instance.burstGauge;
        expSlider.value = (float)PlayerMove.Instance.exp / PlayerMove.Instance.nextExp;
    }

    public void SetBurstSlider(float value)
    {
        burstSlider.value = value;
    }
    public void OnDie()
    {
        goPanel.SetActive(true);
        
        int time = (int)GameManager.Instance.Timer;
        
        gameOverTimeText.text = $"살아남은 시간 : {time / 60:D2}분 {time % 60:D2}초";
        givenDmgText.text = $"가한 데미지 : {GameManager.Instance.GivenDmg:#,##0}";
        takenDmgText.text = $"받은 데미지 : {GameManager.Instance.TakenDmg:#,##0}";
        gameOverLevelText.text = $"현재 레벨 : {PlayerMove.Instance.level}Lv.";
    }

    public void ShowCheatPanel()
    {
        cheatPanel.SetActive(!cheatPanel.activeSelf);
        cPText.text = $"현재 경험치 : {PlayerMove.Instance.exp}\n\n현재 레벨 : {PlayerMove.Instance.level}\n\n레벨 업까지 필요한 경험치 : {(PlayerMove.Instance.nextExp - PlayerMove.Instance.exp < 0 ? 0 : PlayerMove.Instance.nextExp - PlayerMove.Instance.exp)}";
        CancelInvoke(nameof(HideCheatPanel));
        if (cheatPanel.activeSelf)
            Invoke(nameof(HideCheatPanel), 5f);
    }
    private void HideCheatPanel()
    {
        cheatPanel.SetActive(false);
    }
    
    public void StageClear()
    {
        stageClear.SetTrigger(AnimatorID.showText);
        Invoke(nameof(ShowPanel), 4f);
    }

    private void ShowPanel()
    {
        clearPanel.SetActive(true);
        GameManager.Instance.SetCleared(true);
    }

    public void GoMainMenu()
    {
        SceneManager.LoadScene("Main");
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoNextScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

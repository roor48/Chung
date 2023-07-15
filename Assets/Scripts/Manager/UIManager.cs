using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Animator stageStart;
    [SerializeField] private Animator stageClear;
    
    [Header("InPlay UI")]
    public TMP_Text healthText;
    public TMP_Text timeText;
    public TMP_Text scoreText;
    public Slider burstSlider;
    
    [Header("ClearPanel UI")]
    public GameObject clearPanel;
    public TMP_Text panelScore;
    public TMP_Text panelTime;

    [Header("Cheat UI")]
    public GameObject cheatPanel;

    public PlayerMove player;
    public TakeDamage playerHealth;

    private void Awake()
    {
        stageStart.SetTrigger(AnimatorID.showText);
    }

    private void Update()
    {
        healthText.text = $"남은 <color=#FF0000>체력</color> : <b>{playerHealth.health}</b>";
        int time = (int)GameManager.Instance.Timer;
        timeText.text = $"{time / 60:D2} : {time % 60:D2}";
        scoreText.text = $"{GameManager.Instance.Score:#,##0}점";

        panelScore.text = $"점수 : {GameManager.Instance.Score:#,##0}";
        panelTime.text = $"걸린 시간 : {time / 60:D2}분 {time % 60:D2}초";
    }

    public void SetBurstSlider(float value)
    {
        burstSlider.value = value;
    }

    public void ShowCheatPanel()
    {
        cheatPanel.SetActive(true);
        CancelInvoke(nameof(HideCheatPanel));
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
}

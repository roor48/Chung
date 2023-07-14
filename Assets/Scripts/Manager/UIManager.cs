using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text healthText;
    public TMP_Text timeText;

    [SerializeField] private Animator stageStart;
    [SerializeField] private Animator stageClear;
    public GameObject clearPanel;
    
    public TakeDamage playerHealth;

    private void Awake()
    {
        stageStart.SetTrigger("ShowText");
    }

    private void Start()
    {
        GameManager.Instance.Timer = 0f;
    }

    private void Update()
    {
        healthText.text = $"남은 <color=#FF0000>체력</color> : <b>{playerHealth.health}</b>";
        int time = (int)GameManager.Instance.Timer;
        timeText.text = $"{time / 60:D2} : {time % 60:D2}";
    }

    public void StageClear()
    {
        stageClear.SetTrigger("ShowText");
        Invoke(nameof(ShowPanel), 4f);
    }

    private void ShowPanel()
    {
        clearPanel.SetActive(true);
        GameManager.Instance.SetCleared(true);
    }
}

using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    public Animator mainPanel;
    public Animator settingPanel;
    public Animator controlsPanel;
    public Animator explainPanel;

    private void Awake()
    {
        Time.timeScale = 1f;
    }

    public void GoPlayScene()
    {
        SceneManager.LoadScene("Stage 1");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    
    private readonly int showPanel = Animator.StringToHash("show");
    private readonly int hidePanel = Animator.StringToHash("hide");

    private void HideMain()
    {
        mainPanel.SetTrigger(hidePanel);
    }
    private void ShowMain()
    {
        mainPanel.SetTrigger(showPanel);
    }
    
    public void SettingBtn()
    {
        HideMain();
        settingPanel.SetTrigger(showPanel);
    }
    public void HideSettingBtn()
    {
        ShowMain();
        settingPanel.SetTrigger(hidePanel);
    }

    public void ControlsBtn()
    {
        HideMain();
        controlsPanel.SetTrigger(showPanel);
    }
    public void HideControlsBtn()
    {
        ShowMain();
        controlsPanel.SetTrigger(hidePanel);
    }

    public void ExplainBtn()
    {
        HideMain();
        explainPanel.SetTrigger(showPanel);
    }

    public void HideExplainBtn()
    {
        ShowMain();
        explainPanel.SetTrigger(hidePanel);
    }
    
}

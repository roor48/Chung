using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TMP_Text timeText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
            SceneManager.LoadScene("Play 1");
        Debug.Log(GameManager.Instance.time);
    }
}

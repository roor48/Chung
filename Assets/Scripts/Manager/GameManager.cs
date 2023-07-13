using System;
using UnityEngine;

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
    
    public float time;

    private void Awake()
    {
        if (_instance)
            Destroy(this.gameObject);
        else
            _instance = this;
    
        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        time += Time.deltaTime;
    }
}

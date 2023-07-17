using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }
    
    public int maxHealth;
    public int level;
    public string weaponName;
    public int power;
    public int nextExp;
    public int exp;
    public int atkBonus;
    public float curBurstGauge;
    public int petCnt;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
}

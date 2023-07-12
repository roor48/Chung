using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bullet;

    private void Start()
    {
        StartCoroutine(Re_Shoot());
    }

    private WaitForSeconds shootDelay = new(0.5f);

    private IEnumerator Re_Shoot()
    {
        while (true)
        {
            GameManager.Instance.poolManager.GetPool("Player_Bullet");
            yield return shootDelay;
        }
    }
}

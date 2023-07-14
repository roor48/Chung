using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int dmg;

    private void Update()
    {
        if (GameManager.Instance.MainCam.WorldToViewportPoint(transform.position).x > 1.5f ||
            GameManager.Instance.MainCam.WorldToViewportPoint(transform.position).x < -0.5f)
        {
            gameObject.SetActive(false);
        }
    }
}

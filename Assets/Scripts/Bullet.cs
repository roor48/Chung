using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float standardDmg;
    public int dmg;

    private void Update()
    {
        dmg = (int)(standardDmg * transform.localScale.x);

        if (GameManager.Instance.MainCam.WorldToViewportPoint(transform.position).x > 1.5f ||
            GameManager.Instance.MainCam.WorldToViewportPoint(transform.position).x < -0.5f)
        {
            gameObject.SetActive(false);
        }
    }
}

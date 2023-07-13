using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerMap : MonoBehaviour
{
    public Remap[] corners;

    public float speed;
    private bool stopRot = true;
    private void OnEnable()
    {
        // 위치 초기화
        transform.eulerAngles = Vector3.zero;
        corners[0].transform.localPosition = new Vector3(0,-1.92f,-1.43f);
        corners[1].transform.localPosition = new Vector3(17.33f,-1.92f,-5.58f);
        corners[2].transform.localPosition = new Vector3(28.44f,-1.92f,-16.75f);
        corners[3].transform.localPosition = new Vector3(33.79f,-1.92f,-36.72f);
        // No "for" for faster
        corners[0].speed = 0;
        corners[1].speed = 0;
        corners[2].speed = 0;
        corners[3].speed = 0;
        stopRot = true;
    }

    private void Update()
    {
        if (corners[3].transform.localPosition.z >= 35)
        {
            gameObject.SetActive(false);
        }

        if (stopRot)
        {
            transform.position += Time.deltaTime * 10 * Vector3.left;
            if (transform.position.x <= 0)
            {
                stopRot = false;
                transform.position = Vector3.zero;
                // No "for" for faster
                corners[0].speed = 10;
                corners[1].speed = 10;
                corners[2].speed = 10;
                corners[3].speed = 10;
            }

            return;
        }
        
        transform.Rotate(Time.deltaTime * speed * Vector3.down);
        if (transform.eulerAngles.y <= 270f)
        {
            stopRot = true;
            transform.eulerAngles = Vector3.down * 90;
        }
    }
}

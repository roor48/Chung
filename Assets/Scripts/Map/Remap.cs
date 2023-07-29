using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remap : MonoBehaviour
{
    public Transform target;

    public Vector3 pos;
    public float speed;
    public bool stopReturn;

    private void FixedUpdate()
    {
        if (stopReturn)
            return;
        transform.position += Time.fixedDeltaTime * speed  * Vector3.left;
        if (transform.position.x <= -32)
                transform.position = new Vector3(target.position.x + pos.x, pos.y, pos.z); // 가장 오른쪽으로 돌아가기
    }
}

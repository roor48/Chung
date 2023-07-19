using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurstGauge : MonoBehaviour
{
    public float burstValue;

    private Rigidbody rigid;
    private Transform playerTrans;
    private int speed;
    private void OnEnable()
    {
        rigid = GetComponent<Rigidbody>();
        playerTrans = Player.Instance.transform;
        rigid.velocity = Vector3.zero;
        speed = 1;
    }

    private void Update()
    {
        Vector3 dirVec = playerTrans.position - transform.position;
        dirVec.y = 0;
        rigid.velocity = dirVec.normalized * speed++;
        if (speed > 50)
            speed = 50;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        gameObject.SetActive(false);
        Player.Instance.curBurstGauge += burstValue;
    }
}

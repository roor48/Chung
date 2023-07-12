using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    
    private Transform playerTrans;
    private Rigidbody rigid;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        playerTrans = GameObject.FindWithTag("Player").transform;
    }

    private void OnEnable()
    {
        transform.position = playerTrans.position;
        Invoke(nameof(SetInactive), 5f);
    }

    private void FixedUpdate()
    {
        rigid.velocity = Time.fixedDeltaTime * speed * Vector3.right;
    }


    private void SetInactive()
    {
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public int dmg;

    public Rigidbody rigid;

    private Camera _camera;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        rigid.velocity = Time.fixedDeltaTime * speed * Vector3.right;
        if (_camera.WorldToViewportPoint(transform.position).x > 1.5f ||
            _camera.WorldToViewportPoint(transform.position).x < -0.5f)
        {
            gameObject.SetActive(false);
        }
    }
}

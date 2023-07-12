using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remap : MonoBehaviour
{
    public enum Type
    {
        Straight,
        Corner,
    }
    public Type type;
    public Transform target;

    public float speed;
    public bool isCorner;
    
    private Vector3 firstPos;
    private void Awake()
    {
        firstPos = transform.position;
    }

    private void Update()
    {
        transform.position += Time.deltaTime * speed  * Vector3.left;
        if (transform.position.x <= -32)
        {
            if (type == Type.Straight)
            {
                transform.position = new Vector3(target.position.x + 20, -1.92f, -1.43f);
            }
            else
            {
                
            }
        }
    }
}

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

    public Vector3 pos;
    public float speed;
    public bool stopReturn;

    private Vector3 firstPos;
    private void Awake()
    {
        firstPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (stopReturn)
            return;
        transform.position += Time.fixedDeltaTime * speed  * Vector3.left;
        if (transform.position.x <= -32)
        {
            switch (type)
            {
                case Type.Straight:
                    transform.position = new Vector3(target.position.x + pos.x, pos.y, pos.z); // new Vector3(target.position.x + 20, -1.92f, -1.43f);
                    break;
                case Type.Corner:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

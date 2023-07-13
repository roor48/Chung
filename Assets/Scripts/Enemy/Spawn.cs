using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    private Transform[] points;

    private void Awake()
    {
        points = GetComponentsInChildren<Transform>();
    }
}

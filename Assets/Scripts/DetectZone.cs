using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectZone : MonoBehaviour
{
    public List<Transform> detected;

    private void Awake()
    {
        detected = new();
    }

    public Transform GetNearest()
    {
        Transform near = detected[0];
        float nearest = 999f;
        foreach (Transform detect in detected)
        {
            if (Vector3.Distance(transform.position, detect.position) < nearest)
            {
                nearest = Vector3.Distance(transform.position, detect.position);
                near = detect;
            }
        }

        return near;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy") && !other.CompareTag("Boss") || detected.Contains(other.transform))
            return;
        
        detected.Add(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Enemy") && !other.CompareTag("Boss"))
            return;

        detected.Remove(other.transform);
    }
}

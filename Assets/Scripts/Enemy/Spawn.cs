using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawn : MonoBehaviour
{
    private Transform[] points;

    public float spawnDelay;
    public GameObject bossKing;
    private float curDelay;

    private bool isBoss = false;
    private void Awake()
    {
        points = GetComponentsInChildren<Transform>();
    }

    private void Update()
    {
        if (GameManager.Instance.isCleared)
            return;
        if (curDelay > 0)
        {
            curDelay -= Time.deltaTime;
            return;
        }
        curDelay = spawnDelay;

        GameObject enemy = GameManager.Instance.poolManager.GetPool("Enemy_Rabbit");
        enemy.transform.position = points[isBoss ? Random.Range(1, 5) : Random.Range(1, 8)].position;
    }
}

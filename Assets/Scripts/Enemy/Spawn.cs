using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class Spawn : MonoBehaviour
{
    private Transform[] points;
    public GameObject bossObject;

    public float spawnDelay;
    private float curDelay;

    public List<SpawnStruct> spawnList;
    public int spawnIndex;
    public bool spawnEnd;
    private void Awake()
    {
        points = GetComponentsInChildren<Transform>();
        spawnList = new();
        ReadSpawnFile();
    }

    private void ReadSpawnFile()
    {
        // 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;
        
        //f=리스폰 파일 읽기
        TextAsset textFile = Resources.Load("Stage 1") as TextAsset;
        StringReader stringReader = new(textFile.text);

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();

            if (line == null)
                break;
            
            SpawnStruct spawnData = new();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.name = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData );
        }
        
        stringReader.Close();

        spawnDelay = spawnList[0].delay;
    }

    private void Update()
    {
        if (spawnEnd)
            return;
        
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (curDelay < spawnDelay)
        {
            curDelay += Time.deltaTime;
            return;
        }
        curDelay = 0;
        int spawnPoint = spawnList[spawnIndex].point;

        if (spawnList[spawnIndex].name == "Boss")
        {
            bossObject.SetActive(true);
        }
        else
        {
            GameObject enemy = GameManager.Instance.poolManager.GetPool(spawnList[spawnIndex].name);
            enemy.transform.position = points[spawnPoint].position;
        }

        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }
        
        spawnDelay = spawnList[spawnIndex].delay;
    }
}

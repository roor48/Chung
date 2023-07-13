using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StraightMap : MonoBehaviour
{
    public GameObject[] straights;

    private int idx;

    private void Start()
    {
    }

    public GameObject SetCorner()
    {
        GameObject select = straights[0];
        for (int i = 1; i < 4; i++)
        {
            if (select.transform.position.x < straights[i].transform.position.x)
                select = straights[i];
            // 가장 먼거 return
        }
        
        return select;
    }
}

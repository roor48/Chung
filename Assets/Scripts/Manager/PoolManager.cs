using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private Pool[] pools;

    private Dictionary<string, List<GameObject>> objDict;
    private void Awake()
    {
        objDict = new();

        for (int i = 0; i < pools.Length; i++)
        {
            objDict[pools[i].name] = new();
            for (int j = 0; j < pools[i].size; j++)
            {
                objDict[pools[i].name].Add(Instantiate(pools[i].obj, transform));
                objDict[pools[i].name][j].SetActive(false);
            }
        }
    }

    public GameObject GetPool(string target)
    {
        GameObject select = null;
        for (int i = 0; i < objDict[target].Count; i++)
        {
            if (!objDict[target][i].activeSelf)
            {
                select = objDict[target][i];
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(objDict[target][0], transform);
            objDict[target].Add(select);
        }
        select.SetActive(true);
        return select;
    }

    public void DisableEnemy()
    {
        for (int i = 0; i < pools.Length; i++)
        {
            if (pools[i].obj.CompareTag("Enemy") || pools[i].obj.CompareTag("Bullet"))
            {
                for (int j = 0; j < objDict[pools[i].name].Count; j++)
                {
                    objDict[pools[i].name][j].SetActive(false);
                }
            }
        }
    }
}


[System.Serializable]
public class Pool
{
    public string name;
    [Range(1, 500)]
    public int size;
    public GameObject obj;
}
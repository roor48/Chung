using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [SerializeField] private Pool[] pools;

    private Dictionary<string, List<GameObject>> objDict;
    private void Awake()
    {
        objDict = new();
        Instance = this;

        foreach (Pool pool in pools)
        {
            objDict[pool.name] = new();
            for (int j = 0; j < pool.size; j++)
            {
                objDict[pool.name].Add(Instantiate(pool.obj, transform));
                objDict[pool.name][j].SetActive(false);
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
            select.gameObject.SetActive(false);
        }
        select.SetActive(true);
        return select;
    }

    public void DamageEnemy(bool doDisable)
    {
        foreach (Pool pool in pools)
        {
            if (pool.obj.CompareTag("Enemy") || pool.obj.CompareTag("Bullet"))
            {
                for (int j = 0; j < objDict[pool.name].Count; j++)
                {
                    if (!objDict[pool.name][j].activeSelf) continue;

                    if (doDisable || pool.obj.CompareTag("Bullet"))
                        objDict[pool.name][j].SetActive(false);
                    else
                        objDict[pool.name][j].GetComponent<TakeDamage>().GetDamage(100);
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
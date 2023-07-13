using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    public static PoolManager Instance => _instance ? _instance : null;
    
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
    
        if (_instance)
            Destroy(this.gameObject);
        else
            _instance = this;
    
        DontDestroyOnLoad(this.gameObject);
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
}


[System.Serializable]
public class Pool
{
    public string name;
    [Range(1, 500)]
    public int size;
    public GameObject obj;
}
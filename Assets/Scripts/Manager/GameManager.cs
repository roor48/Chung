using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PoolManager poolManager;
    private static GameManager _instance;
    public static GameManager Instance => _instance ? _instance : null;

    private void Awake()
    {
        if (_instance)
            Destroy(this.gameObject);
        else
            _instance = this;
        
        DontDestroyOnLoad(this.gameObject);
    }

}

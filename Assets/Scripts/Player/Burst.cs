using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : MonoBehaviour
{
    private void OnBurst()
    {
        PoolManager.Instance.DisAbleEnemy(false);
    }
}

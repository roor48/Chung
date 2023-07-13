using UnityEngine;

public class Shoot : MonoBehaviour
{
    public float shootDelay;
    private float curDelay;

    public int curPower;
    private void Update()
    {
        if (curDelay > 0)
        {
            curDelay -= Time.deltaTime;
            return;
        }
        curDelay = shootDelay;

        switch (curPower)
        {
            case 0:
                GameObject bullet = PoolManager.Instance.GetPool("Player_Bullet");
                bullet.transform.position = transform.position;
                break;
        }

    }
}

using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Enemy,
        Cube,
        Sphere,
    }
    public BulletType bulletType;
    
    private MeshRenderer meshRend;
    
    public int throughCnt;
    public int curThroughCnt;

    public int stdDmg;
    public int dmg;

    private void Awake()
    {
        meshRend = GetComponent<MeshRenderer>();
        dmg = stdDmg;
    }

    private void OnEnable()
    {
        curThroughCnt = throughCnt;
        if (bulletType != BulletType.Enemy)
        {
            if (bulletType == BulletType.Cube)
                throughCnt = PlayerMove.Instance.curPower == 2 ? 2 : 1;
            int meshIdx = PlayerMove.Instance.level;
            meshIdx = (meshIdx-1)/2 > 2 ? 2 : meshIdx/2;
            meshRend.material = PlayerMove.Instance.bulletMat[meshIdx];
        }
    }

    private void Update()
    {
        if (GameManager.Instance.MainCam.WorldToViewportPoint(transform.position).x > 1.5f ||
            GameManager.Instance.MainCam.WorldToViewportPoint(transform.position).x < -0.5f)
        {
            gameObject.SetActive(false);
        }
    }
}

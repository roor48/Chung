using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType
    {
        Enemy,
        Cube,
        Sphere,
        Torus,
    }
    public BulletType bulletType;
    
    private MeshRenderer meshRend;
    public DetectZone detectZone;
    public Rigidbody rigid;
    
    public int throughCnt;
    public int curThroughCnt;

    public int stdDmg;
    public int dmg;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        meshRend = GetComponent<MeshRenderer>();
        if (bulletType == BulletType.Torus)
            detectZone = transform.GetChild(0).GetComponent<DetectZone>();
        dmg = stdDmg;
    }

    private void OnEnable()
    {
        curThroughCnt = throughCnt;
        if (bulletType != BulletType.Enemy)
        {
            if (bulletType == BulletType.Cube)
                throughCnt = Player.Instance.curPower == 2 ? 2 : 1;
            int meshIdx = Player.Instance.level;
            meshIdx = (meshIdx-1)/2 >= 2 ? 2 : meshIdx/2;
            meshRend.material = Player.Instance.bulletMat[meshIdx];
        }
    }

    private void LateUpdate()
    {
        Vector3 viewPos = GameManager.Instance.MainCam.WorldToViewportPoint(transform.position);
        if (viewPos.x > 1.2f || viewPos.x < -0.2f || viewPos.y > 1.2f || viewPos.y < -0.2f)
        {
            gameObject.SetActive(false);
        }
        
        if (bulletType != BulletType.Torus)
            return;

        if (detectZone.detected.Count > 0)
        {
            Vector3 dirVec = detectZone.GetNearest().position - transform.position;
            dirVec.y = 0;
            rigid.velocity = dirVec.normalized * 7;
        }
        else
        {
            rigid.velocity = Vector3.right * 7;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    private float curDelay;

    public Vector3 followPos;
    public Transform targetTrans;
    private Queue<Vector3> targetPos;
    public int followDelay;

    private void Start()
    {
        targetPos = new();
    }

    public void Update()
    {
        if (GameManager.Instance.isCleared || PlayerMove.Instance.isDead)
            return;
        Follow();
        Shoot();
    }

    private void Follow()
    {
        if (!targetPos.Contains(targetTrans.position))
            targetPos.Enqueue(targetTrans.position);
        
        if (targetPos.Count > followDelay)
            followPos = targetPos.Dequeue();
        else if (targetPos.Count < followDelay)
            followPos = Vector3.down * 10;

        transform.position = followPos;
    }

    private void Shoot()
    {
        if (curDelay > 0)
        {
            curDelay -= Time.deltaTime;
            return;
        }
        curDelay = 2;

        GameObject bullet = PoolManager.Instance.GetPool(PlayerMove.Instance.GetBulletName);
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
        Bullet bulletScript = bullet.GetComponent<Bullet>();

        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bullet.transform.localScale = Vector3.one * 0.3f;
        bulletRigid.velocity = Vector3.zero;
        bulletScript.dmg = bulletScript.stdDmg / 2;

        bulletRigid.AddForce(PlayerMove.Instance.bulletSpd * Vector3.right, ForceMode.Impulse);
    }
}

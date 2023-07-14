using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public enum EnemyList
    {
        Normal,
        MetalHelmet,
        Rabbit,
    }
    public EnemyList enemyList;

    public string bulletName;

    public float bulletSpd;
    public float delay;
    private float curDelay;

    private TakeDamage takeDamage;
    private Transform playerTrnas;
    
    private void Awake()
    {
        takeDamage = GetComponent<TakeDamage>();
        playerTrnas = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (takeDamage.health <= 0)
            return;
        BulletShoot();
    }
    
    private void BulletShoot()
    {
        if (curDelay > 0)
        {
            curDelay -= Time.deltaTime;
            return;
        }
        curDelay = delay;

        switch (enemyList)
        {
            case EnemyList.Normal:
                NormalShoot();
                break;

            case EnemyList.MetalHelmet:
                StartCoroutine(MetalShoot());
                break;
            
            case EnemyList.Rabbit:
                RabbitShoot();
                break;
        }
    }

    private void NormalShoot()
    {
        GameObject bullet = GameManager.Instance.poolManager.GetPool(bulletName);
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bulletRigid.velocity = Vector3.zero;
                
        bullet.transform.LookAt(playerTrnas.position);
                
        Vector3 dirVec = playerTrnas.position - transform.position;
        dirVec.y = 0;

        bulletRigid.AddForce(bulletSpd * dirVec.normalized, ForceMode.Impulse);
    }

    private readonly WaitForSeconds wait_3 = new(0.3f);
    private IEnumerator MetalShoot()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject bullet = GameManager.Instance.poolManager.GetPool(bulletName);
            Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            bulletRigid.velocity = Vector3.zero;
            
            bullet.transform.LookAt(playerTrnas.position);
                
            Vector3 dirVec = playerTrnas.position - transform.position;
            dirVec.y = 0;

            bulletRigid.AddForce(bulletSpd * dirVec.normalized, ForceMode.Impulse);
            yield return wait_3;
        }
    }

    private void RabbitShoot()
    {
        for (int i = -1; i < 2; i++)
        {
            GameObject bullet = GameManager.Instance.poolManager.GetPool(bulletName);
            Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            // bullet.transform.eulerAngles = new Vector3(0, 0, -90);
            bulletRigid.velocity = Vector3.zero;
                
            Vector3 dirVec = playerTrnas.position - transform.position;
            dirVec.y = 0;

            Quaternion rot = Quaternion.LookRotation(dirVec.normalized, Vector3.forward);
            rot.eulerAngles += rot.eulerAngles.y > 0 ? Vector3.up * 90 : Vector3.down * 90;
            if (playerTrnas.position.x > transform.position.x)
                rot.eulerAngles += Vector3.forward * 180;

            rot.eulerAngles += i * 30 * Vector3.up;
            
            bullet.transform.rotation = rot;
            
            bulletRigid.AddForce(bulletSpd * -bullet.transform.up, ForceMode.Impulse);
            dirVec.y = 0;
        }
    }
}

using System;
using System.Collections;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public enum EnemyList
    {
        Normal,
        MetalHelmet,
        Rabbit,
        Cactus,
        Burrow,
        Fierey,
    }
    public EnemyList enemyList;

    public string bulletName;

    public float speed;
    public float bulletSpd;
    public float delay;
    private float curDelay;

    public float burstValue;

    private Animator anim;
    private TakeDamage takeDamage;
    private Transform playerTrans;
    
    private Rigidbody rigid;

    private bool isMaked;
    private void Awake()
    {
        anim = GetComponent<Animator>();
        takeDamage = GetComponent<TakeDamage>();
        rigid = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        playerTrans = PlayerMove.Instance.transform;
    }

    private void OnEnable()
    {
        rigid.velocity = speed * Vector3.left;
        curDelay = delay;
    }
    private void OnDisable()
    {
        if (!isMaked || takeDamage.health > 0)
        {
            isMaked = true;
            return;
        }

        if (SoundManager.Instance.dieSound)
            SoundManager.Instance.dieSound.Play();
        GameObject bead = PoolManager.Instance.GetPool("BurstBead");
        bead.GetComponent<BurstGauge>().burstValue = this.burstValue;
        bead.transform.position = transform.position + Vector3.right * 6.5f;
    }

    private void Update()
    {
        if (takeDamage.health <= 0 || GameManager.Instance.MainCam.WorldToViewportPoint(transform.position).x < 0.1f)
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
            
            case EnemyList.Cactus:
                anim.SetTrigger(attack);
                break;
            case EnemyList.Burrow:
                BurrowShoot();
                break;
            case EnemyList.Fierey:
                FiereyShoot();
                break;
        }
    }

    private void NormalShoot()
    {
        GameObject bullet = PoolManager.Instance.GetPool(bulletName);
        Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        bulletRigid.velocity = Vector3.zero;
                
        bullet.transform.LookAt(playerTrans.position);
                
        Vector3 dirVec = playerTrans.position - transform.position;
        dirVec.y = 0;

        bulletRigid.AddForce(bulletSpd * dirVec.normalized, ForceMode.Impulse);
    }

    private readonly WaitForSeconds wait_3 = new(0.3f);
    private IEnumerator MetalShoot()
    {
        for (int i = 0; i < 3 && takeDamage.health > 0; i++)
        {
            GameObject bullet = PoolManager.Instance.GetPool(bulletName);
            Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            bulletRigid.velocity = Vector3.zero;
            
            bullet.transform.LookAt(playerTrans.position);
                
            Vector3 dirVec = playerTrans.position - transform.position;
            dirVec.y = 0;

            bulletRigid.AddForce(bulletSpd * dirVec.normalized, ForceMode.Impulse);
            yield return wait_3;
        }
    }

    private void RabbitShoot()
    {
        for (int i = -1; i < 2; i++)
        {
            GameObject bullet = PoolManager.Instance.GetPool(bulletName);
            Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            bulletRigid.velocity = Vector3.zero;
                
            Vector3 dirVec = playerTrans.position - transform.position;
            dirVec.y = 0;

            Quaternion rot = Quaternion.LookRotation(dirVec.normalized, Vector3.forward);
            rot.eulerAngles += rot.eulerAngles.y > 0 ? Vector3.up * 90 : Vector3.down * 90;
            if (playerTrans.position.x > transform.position.x)
                rot.eulerAngles += Vector3.forward * 180;

            rot.eulerAngles += i * 30 * Vector3.up;
            
            bullet.transform.rotation = rot;
            
            bulletRigid.AddForce(bulletSpd * -bullet.transform.up, ForceMode.Impulse);
            dirVec.y = 0;
        }
    }

    private readonly Vector3[] cactusDirVec =
        { new Vector3(-1, 0, 0), new Vector3(0, 0, 1), new Vector3(1, 0, 0), new Vector3(0, 0, -1) };

    private readonly int attack = Animator.StringToHash("Attack");
    private void CactusShoot()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject bullet = PoolManager.Instance.GetPool(bulletName);
            bullet.transform.position = transform.position;
            bullet.transform.eulerAngles = new Vector3(0, 90 * i, 90);
            bullet.GetComponent<Rigidbody>().velocity = cactusDirVec[i] * bulletSpd;
        }
    }
    private void StopOnCactusAtk()
    {
        rigid.velocity = Vector3.zero;
    }
    private void MoveOnCactusAtkEnd()
    {
        rigid.velocity = speed * Vector3.left;
    }

    private void BurrowShoot()
    {
        for (int i = -2; i < 3; i++)
        {
            GameObject bullet = PoolManager.Instance.GetPool(bulletName);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody>().velocity = new Vector3(-1, 0, i/2f).normalized * bulletSpd;
        }
    }
    private readonly Vector3[] fiereyDirVec =
        { new Vector3(1, 0, 1), new Vector3(-1, 0, 1), new Vector3(-1, 0, -1), new Vector3(1, 0, -1) };
    private void FiereyShoot()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject bullet = PoolManager.Instance.GetPool(bulletName);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody>().velocity = fiereyDirVec[i] * bulletSpd;
        }
    }
}

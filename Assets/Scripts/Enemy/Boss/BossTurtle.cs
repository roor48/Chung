using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossTurtle : MonoBehaviour
{
    public UIManager uiManager;
    
    private Transform playerTrans;
    private Rigidbody rigid;
    private TakeDamage takeDamage;
    private Animator anim;
    private AudioSource audioSource;

    public AudioClip[] audios;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        takeDamage = GetComponent<TakeDamage>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        
        playerTrans = PlayerMove.Instance.transform;
    }

    private void OnEnable()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        rigid.AddForce(Vector3.left * 7, ForceMode.Impulse);
        yield return new WaitForSeconds(1.5f);
        rigid.velocity = Vector3.zero;
        anim.SetTrigger("Idle");
        StartCoroutine(Think());
    }

    private readonly WaitForSeconds wait3 = new(3f);
    private readonly WaitForSeconds wait_1 = new(0.1f);
    private readonly WaitForSeconds wait__2 = new(0.02f);
    public int patternID = 0;
    private IEnumerator Think()
    {
        if (takeDamage.isDead)
            yield break;
        switch (patternID++)
        {
            case 2:
                Shield();
                while (anim.GetBool(defend) && !takeDamage.isDead)
                    yield return null;

                Dizzy();
                while (anim.GetBool(dizzy) && !takeDamage.isDead)
                    yield return null;
                break;
            
            case 1:
                GameObject bullet = PoolManager.Instance.GetPool("Bullet_Cone");
                Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
                bulletRigid.velocity = Vector3.zero;
                bullet.transform.position = transform.position + Vector3.left * 3;
                Vector3 dirVec = default;
                for (int i = 0; i < 150 && !takeDamage.isDead; i++)
                {
                    dirVec = playerTrans.position - bullet.transform.position;
                    dirVec.y = 0;
                    bullet.transform.LookAt(playerTrans);
                    bullet.transform.eulerAngles = new Vector3(0, bullet.transform.eulerAngles.y+90, 90);

                    yield return wait__2;
                }

                bullet.GetComponent<Rigidbody>().velocity = dirVec.normalized * 20;
                break;
            case 0:
                for (int i = 0; i < 64 && !takeDamage.isDead; i++)
                {
                    bullet = PoolManager.Instance.GetPool("Bullet_Cone");
                    bulletRigid = bullet.GetComponent<Rigidbody>();
                    bulletRigid.velocity = Vector3.zero;
                    bullet.transform.position = transform.position;
                    dirVec = new Vector3(Mathf.Cos(Mathf.PI * 4 * i / 63), 0, Mathf.Sin(Mathf.PI * 4 * i / 63));

                    Quaternion q = Quaternion.LookRotation(dirVec);
                    q.eulerAngles = new Vector3(0, q.eulerAngles.y + 90, 90);
                    bullet.transform.rotation = q;
                    bulletRigid.velocity = dirVec * 8;
                    yield return wait_1;
                }
                break;
        }

        if (patternID > 2)
            patternID = 0;

        yield return wait3;
        StartCoroutine(Think());
    }

    #region Shield
    private readonly int defend = Animator.StringToHash("Defend");
    private void Shield()
    {
        anim.SetBool(defend, true);
        Invoke(nameof(StopShield), 5f);
    }
    private void StopShield()
    {
        anim.SetBool(defend, false);
    }
    #endregion

    #region Dizzy
    private readonly int dizzy = Animator.StringToHash("Dizzy");
    private void Dizzy()
    {
        anim.SetBool(dizzy, true);
        Invoke(nameof(StopDizzy), 5f);
    }
    private void StopDizzy()
    {
        anim.SetBool(dizzy, false);
    }
    #endregion

    public void SetInActive()
    {
        gameObject.SetActive(false);
        uiManager.StageClear();
    }
}

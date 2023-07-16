using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BossKing : MonoBehaviour
{
    public UIManager uiManager;
    
    private Rigidbody rigid;
    private Animator anim;
    private AudioSource audioSource;
    
    public AudioClip[] audios;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        rigid.AddForce(Vector3.left * 10, ForceMode.Impulse);
        yield return new WaitForSeconds(0.44f);
        rigid.velocity = Vector3.zero;
        anim.SetTrigger("StartMove");
    }

    private readonly int doJump = Animator.StringToHash("doJump");
    private readonly int doAttack = Animator.StringToHash("doAttack");
    private readonly WaitForSeconds idleTime = new(2f);
    private readonly WaitForSeconds wait_5 = new(0.5f);

    private int patternID = 0;
    private IEnumerator Think()
    {
        yield return wait_5;
        switch (patternID)
        {
            case 0: // Jump
                int ranJump = Random.Range(1, 4);
                for (int i = 0; i < ranJump; i++)
                {
                    anim.SetTrigger(doJump);
                    yield return wait_5;
                }
                break;
            
            case 1: // Attack
                anim.SetTrigger(doAttack);
                break;
        }

        patternID++;
        if (patternID > 1)
            patternID = 0;

        yield return idleTime;
        StartCoroutine(Think());
    }

    private void JumpPattern() // 애니메이션
    {
        audioSource.clip = audios[0];
        audioSource.Play();

        int ranBullet = Random.Range(0,2);
        ranBullet = ranBullet == 0 ? 30 : 37;
        for (int j = 0; j < ranBullet; j++)
        {
            GameObject bullet = PoolManager.Instance.GetPool("Bullet_King");
            Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
            bullet.transform.position = transform.position;
            bulletRigid.velocity = Vector3.zero;

            Vector3 dirVec = new Vector3(Mathf.Cos(Mathf.PI * 2 * j / ranBullet), 0, Mathf.Sin(Mathf.PI * 2 * j / ranBullet));
            bulletRigid.AddForce(dirVec * 5, ForceMode.Impulse);
        }
    }
    private void AttackPattern() // 애니메이션
    {
        for (int i = 0; i < 15; i++)
        {
            GameObject bullet = PoolManager.Instance.GetPool("Bullet_King");
            Rigidbody bulletRigid = bullet.GetComponent<Rigidbody>();
            bullet.transform.position = transform.position + Vector3.left * 3;
            bulletRigid.velocity = Vector3.zero;

            Vector3 dirVec = new Vector3(-3, 0, Mathf.Cos(Mathf.PI * 2 * i / 15) * 2);
            bulletRigid.AddForce(dirVec.normalized * 5, ForceMode.Impulse);
        }
    }
    
    private void SetPosition() // 애니메이션
    {
        anim.SetTrigger("doIdle");
        transform.position = new Vector3(5.4f, 1.5f, 0);
        StartCoroutine(Think());
    }
    private void SetInActive() // 애니메이션
    {
        gameObject.SetActive(false);
        uiManager.StageClear();
    }
}

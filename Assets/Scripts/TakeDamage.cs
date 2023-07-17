using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int xp;
    public bool noDamage;
    public bool disableOnDie;

    private Rigidbody rigid;
    private Animator anim;
    private AudioSource audioSource;
    private bool isDead;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health = maxHealth;
        isDead = false;
    }

    private readonly int defend = Animator.StringToHash("Defend");
    private readonly int defendGetHit = Animator.StringToHash("DefendGetHit");
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Bullet") || GameManager.Instance.isCleared || isDead || noDamage)
            return;

        Bullet bulletScript = other.GetComponent<Bullet>();
        if (bulletScript == null)
            return;

        if (bulletScript.curThroughCnt-- <= 0)
            other.gameObject.SetActive(false);
        
        if (anim.GetBool(defend))
        {
            anim.SetTrigger(defendGetHit);
            return;
        }
        GetDamage(bulletScript.dmg);
    }

    public void GetDamage(int dmg)
    {
        health -= dmg;
        if (CompareTag("Player"))
            GameManager.Instance.TakenDmg += dmg;
        else if (CompareTag("Enemy") || CompareTag("Boss"))
            GameManager.Instance.GivenDmg += dmg;

        if (health <= 0)
        {
            health = 0;
            isDead = true;
            rigid.velocity = Vector3.zero;
            if (disableOnDie)
            {
                gameObject.SetActive(false);
                return;
            }
            if (!gameObject.CompareTag("Player")) // 플레이어가 아니면 점수 추가 및 경험치 획득
            {
                GameManager.Instance.Score += maxHealth;
                PlayerMove.Instance.SetLevel(xp);

                if (gameObject.CompareTag("Enemy"))
                    CreateItem();
                else if (gameObject.CompareTag("Boss"))
                {
                    Debug.Log("Boss Dead!");
                    PoolManager.Instance.DamageEnemy(true);
                }
            }
            else
            {
                gameObject.GetComponent<PlayerMove>().OnDie();
            }
            
            if (anim == null)
                SetInActive(); // 애니메이터가 없으면 바로 호출
            else
                anim.SetTrigger(AnimatorID.onDie);
        }
    }

    private void CreateItem()
    {
        int ranItem = Random.Range(0, 100);
        string itemName;
        switch (ranItem)
        {
            case < 10:
                itemName = "Item_Speed";
                break;
            case < 17:
                itemName = "Item_Pet";
                break;
            case < 22:
                itemName = "Weapon_Cube";
                break;
            case < 27:
                itemName = "Weapon_Sphere";
                break;
            default:
                return;
        } 

        PoolManager.Instance.GetPool(itemName).transform.position = transform.position;
    }

    // 애니메이션에서 호출
    private void SetInActive()
    {
        gameObject.SetActive(false);
    }
}

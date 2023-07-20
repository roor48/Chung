using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int maxHealth;
    public int health;
    public int xp;
    public bool noDamage;
    public bool disableOnDie;
    public bool isTurtle;

    private Rigidbody rigid;
    private Animator anim;
    private AudioSource audioSource;
    public bool isDead;

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
        
        if (isTurtle) // 거북이이면서
            if (anim.GetBool(defend)) // defend 애니메이션 중 일때
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
                SetInActive();
                return;
            }
            if (!gameObject.CompareTag("Player")) // 플레이어가 아니면 점수 추가 및 경험치 획득
            {
                GameManager.Instance.Score += maxHealth;
                Player.Instance.SetLevel(xp);

                if (gameObject.CompareTag("Enemy"))
                    CreateItem();
                else if (gameObject.CompareTag("Boss"))
                {
                    Debug.Log("Boss Dead!");
                    PoolManager.Instance.DisAbleEnemy(true);
                }
            }
            else
            {
                gameObject.GetComponent<Player>().OnDie();
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
            case < 6:
                itemName = "Item_Speed";
                break;
            case < 10:
                itemName = "Item_Pet";
                break;
            case < 15:
                itemName = "Weapon_Cube";
                break;
            case < 20:
                itemName = "Weapon_Sphere";
                break;
            case < 24:
                itemName = "Weapon_Torus";
                break;
            case < 29:
                itemName = "Item_Barrier";
                break;
            case < 31:
                itemName = "Item_HealFull";
                break;
            case < 35:
                itemName = "Item_HealLow";
                break;

            default:
                return;
        }

        PoolManager.Instance.GetPool(itemName).transform.position = transform.position;
    }

    // 애니메이션에서 호출
    private void SetInActive()
    {
        transform.position += Vector3.right * 100;
        gameObject.SetActive(false);
    }
}

using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int maxHealth;
    public int health;
    // public int score;
    public bool noDamage;

    private Animator anim;
    private bool isDead;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health = maxHealth;
        isDead = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet") && !isDead && !noDamage)
        {
            Bullet bulletScript = other.GetComponent<Bullet>();
            GetDamage(bulletScript.dmg);
            if (bulletScript.curThroughCnt-- <= 0)
                other.gameObject.SetActive(false);
        }
    }

    public void GetDamage(int dmg)
    {
        health -= dmg;
        Debug.Log(name + health);
        if (health <= 0)
        {
            isDead = true;
            GameManager.Instance.Score += maxHealth;
            if (gameObject.CompareTag("Boss"))
                PoolManager.Instance.DamageEnemy(true);
            // if (gameObject.CompareTag("Enemy"))
            //     CreateItem();
            
            if (anim == null)
                SetInActive(); // 애니메이터가 없으면 바로 호출
            else
                anim.SetTrigger(AnimatorID.onDie);
        }
    }

    private void CreateItem()
    {
        int ranItem = Random.Range(0, 4);
        string itemName;
        switch (ranItem)
        {
            case <49:
                itemName = "Speed";
                break;
            
            default:
                itemName = "Coin";
                break;
        }

        PoolManager.Instance.GetPool(itemName).transform.position = transform.position;
    }

    // 애니메이션에서 호출
    private void SetInActive()
    {
        gameObject.SetActive(false);
    }
}

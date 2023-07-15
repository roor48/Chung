using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int maxHealth;
    public int health;
    // public int score;

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
        if (other.CompareTag("Bullet") && !isDead)
        {
            GetDamage(other.GetComponent<Bullet>().dmg);
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
            if (anim == null)
                SetInActive(); // 애니메이터가 없으면 바로 호출
            else
                anim.SetTrigger(AnimatorID.onDie);
        }
    }

    // 애니메이션에서 호출
    private void SetInActive()
    {
        gameObject.SetActive(false);
    }
}

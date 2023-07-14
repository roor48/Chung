using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int maxHealth;
    public int health;

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
        if (health <= 0)
        {
            isDead = true;
            if (gameObject.CompareTag("Boss"))
                GameManager.Instance.poolManager.DisableEnemy();
            if (anim == null)
                SetInActive(); // 애니메이터가 없으면 바로 호출
            else
                anim.SetTrigger(AnimatorID.onDie);
        }
    }

    // 애니메이션에서 컨트롤
    private void SetInActive()
    {
        gameObject.SetActive(false);
    }
}

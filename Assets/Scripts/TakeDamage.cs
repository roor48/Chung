using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    public int maxHealth;
    public int health;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CompareTag("Bullet"))
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
            if (anim == null)
                SetInActive(); // 애니메이터가 없으면 바로 삭제
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

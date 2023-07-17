using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTurtle : MonoBehaviour
{
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
    }
}

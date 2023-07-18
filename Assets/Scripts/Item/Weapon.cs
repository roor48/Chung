using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type
    {
        Cube,
        Sphere,
        Torus,
    }
    public Type type;
    
    public float speed;
    private Rigidbody rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rigid.velocity = Vector3.left * speed;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;
        
        gameObject.SetActive(false);
        PlayerMove.Instance.ChangeWeapon("Bullet_Player_" + type);
    }
}

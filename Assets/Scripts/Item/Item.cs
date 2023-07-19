using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemName
    {
        Coin,
        Speed,
        Pet,
        Barrier,
        HealLow,
        HealFull,
    }
    public ItemName itemName;
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
        switch (itemName)
        {
            case ItemName.Coin:
                GameManager.Instance.Score += 500;
                break;
            
            case ItemName.Speed:
                Player.Instance.SetSpeed();
                break;

            case ItemName.Pet:
                Player.Instance.createPet.MakePet();
                break;
            
            case ItemName.Barrier:
                Player.Instance.DoBarrier();
                break;
            case ItemName.HealLow:
                Player.Instance.HealLow();
                break;
            case ItemName.HealFull:
                Player.Instance.HealFull();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

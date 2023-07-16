using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemName
    {
        Coin,
        Speed,
        Pet,
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
                PlayerMove.Instance.SetSpeed();
                break;

            case ItemName.Pet:
                PlayerMove.Instance.createPet.MakePet();
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

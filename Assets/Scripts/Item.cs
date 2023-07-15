using UnityEngine;

public class Item : MonoBehaviour
{
    public enum ItemName
    {
        Coin,
        Weapon,
        Speed,
        
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
         
            case ItemName.Weapon:
                
                break;
            
            case ItemName.Speed:
                other.GetComponent<PlayerMove>().SetSpeed();
                break;
        }
    }
}

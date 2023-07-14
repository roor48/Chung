using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed;
    private Rigidbody rigid;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rigid.velocity = Vector3.zero;
        rigid.AddForce(speed * Vector3.left, ForceMode.Impulse);
    }
}

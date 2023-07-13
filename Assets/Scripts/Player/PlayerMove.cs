using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float speed;
    public float minYPos, maxYPos;

    private Rigidbody rigid;
    private Animator anim;

    private Vector2 inputVec;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Vector3 camPos = GameManager.Instance.MainCam.WorldToViewportPoint(transform.position);
        if ((camPos.x < 0 && inputVec.x < 0) || (camPos.x > 1 && inputVec.x > 0))
            rigid.velocity = new Vector3(0, 0, inputVec.y);
        if ((camPos.y < minYPos && inputVec.y < 0) || (camPos.y > maxYPos && inputVec.y > 0))
            rigid.velocity = new Vector3(inputVec.x, 0, 0);
    }

    private readonly int moveDir = Animator.StringToHash("moveDir");

    private void OnMove(InputValue value)
    {
        inputVec = value.Get<Vector2>();
    
        rigid.velocity = new Vector3(inputVec.x, 0, inputVec.y) * speed;
    
        // Animator
        int _moveDir = Mathf.RoundToInt(inputVec.x);
        if (_moveDir == 0 && inputVec.y != 0)
            _moveDir = 1;
        anim.SetInteger(moveDir, _moveDir);
    }
}

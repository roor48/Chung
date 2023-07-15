using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rigid;
    private Animator anim;
    
    [Header("Move")]
    public float speed;
    public float minYPos, maxYPos;
    private Vector2 inputVec;

    [Header("Shoot")]
    public float bulletSpd;
    public float shootDelay;
    private float curDelay;
    public int curPower;
    
    [Header("Burst")]
    public Animator burstObj;
    public float burstDelay;
    private float curBurstDelay;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance.isCleared)
        {
            rigid.velocity = Vector3.zero;
            return;
        }
        Move();
        if (curBurstDelay > 0)
            curBurstDelay -= Time.deltaTime;
        Shoot();
    }

    #region Move
    private void Move()
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
    #endregion

    #region Shoot
    private void Shoot()
    {
        if (curDelay > 0)
        {
            curDelay -= Time.deltaTime;
            return;
        }
        curDelay = shootDelay;

        GameObject bullet;
        Rigidbody bulletRigid;
        switch (curPower)
        {
            case 0:
                bullet = PoolManager.Instance.GetPool("Bullet_Player1");
                bulletRigid = bullet.GetComponent<Rigidbody>();
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;
                bullet.transform.localScale = Vector3.one * 0.5f;
                bulletRigid.velocity = Vector3.zero;
                    
                bulletRigid.AddForce(bulletSpd * Vector3.right, ForceMode.Impulse);
                break;
            case 1:
                for (int i = -1; i < 2; i += 2)
                {
                    bullet = PoolManager.Instance.GetPool("Bullet_Player1");
                    bulletRigid = bullet.GetComponent<Rigidbody>();
                    bullet.transform.position = transform.position + i / 2f * Vector3.forward;
                    bullet.transform.rotation = Quaternion.identity;
                    bullet.transform.localScale = Vector3.one * 0.5f;
                    bulletRigid.velocity = Vector3.zero;
                    
                    bulletRigid.AddForce(bulletSpd * Vector3.right, ForceMode.Impulse);
                }
                break;
            case 2:
                for (int i = -1; i < 2; i += 2)
                {
                    bullet = PoolManager.Instance.GetPool("Bullet_Player1");
                    bulletRigid = bullet.GetComponent<Rigidbody>();
                    bullet.transform.position = transform.position + i / 2f * Vector3.forward;
                    bullet.transform.rotation = Quaternion.identity;
                    bullet.transform.localScale = Vector3.one / 3;
                    bulletRigid.velocity = Vector3.zero;
                    
                    bulletRigid.AddForce(bulletSpd * Vector3.right, ForceMode.Impulse);
                }

                goto case 0;
            case 3:
                break;
        }
    }

    private readonly int doBurst = Animator.StringToHash("doBurst");
    #endregion

    #region Burst
    private void OnBurst()
    {
        if (curBurstDelay > 0)
            return;
        curBurstDelay = burstDelay;
        burstObj.SetTrigger(doBurst);
    }
    #endregion
}

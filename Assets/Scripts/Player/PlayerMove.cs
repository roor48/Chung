using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody rigid;
    private Animator anim;
    private TakeDamage takeDamage;
    
    [Header("Manager")]
    public UIManager uiManager;

    [Header("Player Stats")]
    public int level;
    public int[] xpArr = { 10, 30, 60 };
    public int xp;
    
    [Header("Move")]
    public float stdSpeed;
    public float speed;
    public float minYPos, maxYPos;
    private Vector2 inputVec;

    [Header("Shoot")]
    private string bulletName;
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
        takeDamage = GetComponent<TakeDamage>();

        speed = stdSpeed;
        bulletName = GameManager.Instance.bulletName;
    }

    private void Update()
    {
        Cheats();
        if (GameManager.Instance.isCleared)
        {
            rigid.velocity = Vector3.zero;
            return;
        }
        Move();
        if (curBurstDelay > 0)
        {
            uiManager.SetBurstSlider(curBurstDelay / burstDelay);
            curBurstDelay -= Time.deltaTime;
        }
        else
        {
            uiManager.SetBurstSlider(0f);
        }
        Shoot();
    }

    #region Cheat
    private void Cheats()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            takeDamage.noDamage = true;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            SetLevel(1);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            SetLevel(-1);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            takeDamage.health = takeDamage.maxHealth;
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            string[] itemNames = { "Player_Bullet_Cube", "Player_Bullet_Sphere" };
            PoolManager.Instance.GetPool(itemNames[Random.Range(0, itemNames.Length)]).transform.position = new Vector3(7, 1.5f, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            curBurstDelay = 0;
            uiManager.SetBurstSlider(0f);
        }
        else if (Input.GetKeyDown(KeyCode.F7))
        {
            SceneManager.LoadScene("Stage " + SceneManager.GetActiveScene().name == "Stage 1" ? 2 : 1);
        }
        else if (Input.GetKeyDown(KeyCode.F8))
        {
            uiManager.ShowCheatPanel();
        }
    }
    #endregion
    
    #region Move
    public void SetSpeed(float val)
    {
        speed = stdSpeed*2;

        CancelInvoke(nameof(ResetSpeed));
        Invoke(nameof(ResetSpeed), 10f);
    }
    private void ResetSpeed()
    {
        speed = stdSpeed;
    }
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
        Bullet bulletScript;

        switch (curPower)
        {
            case 0:
                bullet = PoolManager.Instance.GetPool(bulletName);
                bulletRigid = bullet.GetComponent<Rigidbody>();
                bulletScript = bullet.GetComponent<Bullet>();
                
                bullet.transform.position = transform.position;
                bullet.transform.rotation = Quaternion.identity;
                bullet.transform.localScale = Vector3.one * 0.5f;
                bulletRigid.velocity = Vector3.zero;
                bulletScript.dmg = 10;
                
                bulletRigid.AddForce(bulletSpd * Vector3.right, ForceMode.Impulse);
                break;
            case 1:
                for (int i = -1; i < 2; i += 2)
                {
                    bullet = PoolManager.Instance.GetPool(bulletName);
                    bulletRigid = bullet.GetComponent<Rigidbody>();
                    bulletScript = bullet.GetComponent<Bullet>();
                    
                    bullet.transform.position = transform.position + i / 2f * Vector3.forward;
                    bullet.transform.rotation = Quaternion.identity;
                    bullet.transform.localScale = Vector3.one * 0.5f;
                    bulletRigid.velocity = Vector3.zero;
                    bulletScript.dmg = 10;
                    
                    bulletRigid.AddForce(bulletSpd * Vector3.right, ForceMode.Impulse);
                }
                break;
            case 2:
                for (int i = -1; i < 2; i += 2)
                {
                    bullet = PoolManager.Instance.GetPool(bulletName);
                    bulletRigid = bullet.GetComponent<Rigidbody>();
                    bulletScript = bullet.GetComponent<Bullet>();
                    
                    bullet.transform.position = transform.position + i / 2f * Vector3.forward;
                    bullet.transform.rotation = Quaternion.identity;
                    bullet.transform.localScale = Vector3.one / 3;
                    bulletRigid.velocity = Vector3.zero;
                    bulletScript.dmg = 6;
                    
                    bulletRigid.AddForce(bulletSpd * Vector3.right, ForceMode.Impulse);
                }

                goto case 0;
            case 3:
                break;
        }
    }

    public void ChangeWeapon(string _bulletName)
    {
        bulletName = _bulletName;
    }
    #endregion

    #region Burst
    private readonly int doBurst = Animator.StringToHash("doBurst");
    public void OnBurst()
    {
        if (curBurstDelay > 0 || GameManager.Instance.isCleared)
            return;
        curBurstDelay = burstDelay;
        burstObj.SetTrigger(doBurst);
    }
    #endregion

    #region Level

    private void SetLevel(int lv)
    {
        level += lv;
    }
    #endregion
}

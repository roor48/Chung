using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerMove : MonoBehaviour
{
    public static PlayerMove Instance { get; private set; }
    
    private Rigidbody rigid;
    private Animator anim;
    private AudioSource audioSource;
    public TakeDamage takeDamage;
    
    [Header("Manager")]
    public UIManager uiManager;

    [Header("Sounds")]
    public AudioClip[] audios;

    [Header("Player Stats")]
    public int level;
    public int nextExp = 15;
    public int exp;
    public int atkBonus;
    public bool isDead;
    
    [Header("Move")]
    public float stdSpeed;
    public float speed;
    public float minYPos, maxYPos;
    private Vector2 inputVec;

    [Header("Shoot")]
    public Material[] bulletMat;
    public string bulletName;
    public float bulletSpd;
    public float shootDelay;
    private float curDelay;
    public int curPower;

    [Header("Burst")]
    public Animator burstObj;
    public float burstGauge;
    public float curBurstGauge;

    [Header("Pet")]
    public CreatePet createPet;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        takeDamage = GetComponent<TakeDamage>();

        Instance = this;
        speed = stdSpeed;
        bulletName = "Bullet_Player_Sphere";
        takeDamage.maxHealth = PlayerStats.Instance.maxHealth;
        level = PlayerStats.Instance.level;
        bulletName = PlayerStats.Instance.weaponName;
        curPower = PlayerStats.Instance.power;
        nextExp = PlayerStats.Instance.nextExp;
        exp = PlayerStats.Instance.exp;
        atkBonus = PlayerStats.Instance.atkBonus;
        curBurstGauge = PlayerStats.Instance.curBurstGauge;

        int pets = PlayerStats.Instance.petCnt;
        for (int i = 0; i < pets; i++)
            createPet.MakePet();
    }

    private void Update()
    {
        Cheats();
        if (GameManager.Instance.isCleared || isDead)
        {
            rigid.velocity = Vector3.zero;
            return;
        }

        if (Input.GetKey(KeyCode.F))
            Time.timeScale = 10f;
        else
            Time.timeScale = uiManager.pausePanel.activeSelf ? 0 : 1;
        LimitMove();
        Shoot();

    }

    #region Cheat
    private void Cheats()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            takeDamage.noDamage = !takeDamage.noDamage;
        }
        else if (Input.GetKeyDown(KeyCode.F2))
        {
            LevelUp(1);
        }
        else if (Input.GetKeyDown(KeyCode.F3))
        {
            LevelUp(-1);
        }
        else if (Input.GetKeyDown(KeyCode.F4))
        {
            takeDamage.health = takeDamage.maxHealth;
        }
        else if (Input.GetKeyDown(KeyCode.F5))
        {
            string[] itemNames = { "Weapon_Cube", "Weapon_Sphere" };
            PoolManager.Instance.GetPool(itemNames[Random.Range(0, itemNames.Length)]).transform.position = new Vector3(7, 1.5f, 0f);
        }
        else if (Input.GetKeyDown(KeyCode.F6))
        {
            curBurstGauge = burstGauge;
            uiManager.SetBurstSlider(0f);
        }
        else if (Input.GetKeyDown(KeyCode.F7))
        {
            SceneManager.LoadScene("Stage " + (SceneManager.GetActiveScene().name == "Stage 1" ? 2 : 1));
        }
        else if (Input.GetKeyDown(KeyCode.F8))
        {
            uiManager.ShowCheatPanel();
        }
        else if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            uiManager.pausePanel.SetActive(!uiManager.pausePanel.activeSelf);
        }
    }
    #endregion
    
    #region Move
    public void SetSpeed()
    {
        speed = stdSpeed*2;
        rigid.velocity = new Vector3(inputVec.x, 0, inputVec.y) * speed;

        CancelInvoke(nameof(ResetSpeed));
        Invoke(nameof(ResetSpeed), 10f);
    }
    private void ResetSpeed()
    {
        speed = stdSpeed;
        rigid.velocity = new Vector3(inputVec.x, 0, inputVec.y) * speed;
    }
    private void LimitMove()
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

    public string GetBulletName => bulletName;

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
                bulletScript.dmg = (int)(bulletScript.stdDmg + bulletScript.stdDmg * (atkBonus / 10f));
                
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
                    bulletScript.dmg = (int)(bulletScript.stdDmg + bulletScript.stdDmg * (atkBonus / 10f));
                    
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
                    bullet.transform.localScale = Vector3.one * 0.3f;
                    bulletRigid.velocity = Vector3.zero;
                    bulletScript.dmg = (int)(bulletScript.stdDmg * 0.6f + bulletScript.stdDmg * (atkBonus / 10f));
                    
                    bulletRigid.AddForce(bulletSpd * Vector3.right, ForceMode.Impulse);
                }

                goto case 0;
        }
    }

    public void ChangeWeapon(string _bulletName)
    {
        audioSource.clip = audios[0];
        audioSource.Play();
        if (bulletName == _bulletName)
        {
            curPower++;
            if (curPower > 2)
            {
                curPower = 2;
                GameManager.Instance.Score += 500;
            }

            return;
        }

        curPower = 0;
        bulletName = _bulletName;
    }
    #endregion

    #region Burst
    private readonly int doBurst = Animator.StringToHash("doBurst");
    public void OnBurst()   
    {
        if (curBurstGauge < burstGauge || GameManager.Instance.isCleared)
            return;
        curBurstGauge = 0;
        burstObj.SetTrigger(doBurst);
    }
    #endregion

    #region Level
    public void SetLevel(int xp)
    {
        exp += xp;
        if (exp >= nextExp)
        {
            LevelUp(exp/nextExp);
            exp %= nextExp;
            nextExp *= 2;
        }

    }

    private readonly int levelUp = Animator.StringToHash("LevelUp");
    private void LevelUp(int val)
    {
        level += val;       // 레벨 업 시 스탯 상승
        uiManager.levelUpAnim.SetTrigger(levelUp);
        takeDamage.maxHealth += 20;
        takeDamage.health += takeDamage.maxHealth / 3;
        if (takeDamage.health > takeDamage.maxHealth)
            takeDamage.health = takeDamage.maxHealth;
        atkBonus += 1;


    }
    #endregion

    public void OnDie()
    {
        uiManager.OnDie();
        isDead = true;
    }
}

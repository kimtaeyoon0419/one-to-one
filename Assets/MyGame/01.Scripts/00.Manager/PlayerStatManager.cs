using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;


public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager instance;

    [Header("플레이어 움직임스텟")]
    public float speed;
    public float JumpPoawer;

    [Header("플레이어 체력스텟")]
    public int ArmorDurability = 0;

    [Header("플레이어 공격스텟")]
    public int AttackPower = 1;
    public int CurBulletCount { get; private set; }
    public int MaxBublletCount;
    public float bulletshotCoolTime;
    public float bulletshotCurTime;
    public bool isDie = false;

    #region Unity_Function
    private void OnEnable()
    {
        SceneManager.sceneLoaded += _OnSceneLoaded;
    }

    void Start()
    {
        CurBulletCount = MaxBublletCount;
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    void Update()
    {
        if (bulletshotCurTime > 0)
        {
            bulletshotCurTime -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            CurBulletCount = MaxBublletCount;
        }
    }
    #endregion

    #region Private_Function
    private void _OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        // 플레이어 스탯 초기화
        if (scene.name == "Main" && scene.name == "Stage_1")
        {
            InitializeStats();
        }
    }
    #endregion

    #region Public_Function
    public void InitializeStats()
    {
        CurBulletCount = MaxBublletCount;
    }
    public void UseBullet()
    {
        CurBulletCount--; // 탄 소비
    }
    #endregion
}

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
    public int CurBulletCount;
    public int MaxBublletCount;
    public float bulletshotCoolTime;
    public float bulletshotCurTime;
    public bool isDie = false;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
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
    public void InitializeStats()
    {
        CurBulletCount = MaxBublletCount;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        // 플레이어 스탯 초기화
        if (scene.name == "Main" && scene.name == "Stage_1")
        {
            InitializeStats();
        }
    }
}

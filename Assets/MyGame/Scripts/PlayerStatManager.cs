using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;


public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager instance;

    [Header("�÷��̾� �����ӽ���")]
    public float speed;
    public float JumpPoawer;

    [Header("�÷��̾� ü�½���")]
    [SerializeField]public int MaxHp;
    public int CurHp;

    [Header("�÷��̾� ���ݽ���")]
    public int AttackPower = 1;
    public int CurBulletCount;
    public int MaxBublletCount;
    public float bulletshotCoolTime;
    public float bulletshotCurTime;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        CurHp = MaxHp;
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
        CurHp = MaxHp;
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
        if (Input.GetKeyDown(KeyCode.S) && CurHp > 0)
        {
            CurHp--;
        }
        if (Input.GetKeyDown(KeyCode.D) && CurHp < 4)
        {
            CurHp++;
        }
    }
    public void InitializeStats()
    {
        CurHp = MaxHp;
        CurBulletCount = MaxBublletCount;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log(scene.name);
        // �÷��̾� ���� �ʱ�ȭ
        if (scene.name == "Main" && scene.name == "Stage_1")
        {
            InitializeStats();
        }
    }

    public void DownHP()
    {
        CurHp -= 1;
    }
}

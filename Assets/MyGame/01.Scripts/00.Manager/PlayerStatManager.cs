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
    public int ArmorDurability = 0;

    [Header("�÷��̾� ���ݽ���")]
    public int AttackPower = 1;
    public int CurBulletCount { get; private set; }
    public int MaxBublletCount;
    public float bulletshotCoolTime;
    public float bulletshotCurTime;

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
        // �÷��̾� ���� �ʱ�ȭ
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
        CurBulletCount--; // ź �Һ�
    }
    #endregion
}

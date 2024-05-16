using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;


public class PlayerStatManager : MonoBehaviour
{
    //public static PlayerStatManager instance;

    [Header("�÷��̾� �����ӽ���")]
    public float speed;
    public float JumpPoawer;

    [Header("�÷��̾� ü�½���")]
    public int ArmorDurability = 0;

    [Header("�÷��̾� ���ݽ���")]
    public int attackPower = 1;
    public int curBulletCount { get; private set; }
    public int maxBublletCount;
    public float bulletshotCoolTime;
    public float bulletshotCurTime;

    #region Unity_Function

    void Start()
    {
        //curBulletCount = maxBublletCount;
        //if (instance == null)
        //{
        //    instance = this;
        //    DontDestroyOnLoad(this.gameObject);
        //}
        //else
        //{
        //    Destroy(this);
        //}
    }

    void Update()
    {
        if (bulletshotCurTime > 0)
        {
            bulletshotCurTime -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            curBulletCount = maxBublletCount;
            Debug.Log("���ݷ� : " + attackPower + "�̵��ӵ� : " + speed);
        }
    }
    #endregion

    #region Private_Function
    //private void _OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    Debug.Log(scene.name);
    //    // �÷��̾� ���� �ʱ�ȭ
    //    if (scene.name == "Main" && scene.name == "Stage_1")
    //    {
    //        InitializeStats();
    //    }
    //}
    #endregion

    #region Public_Function
    public void InitializeStats()
    {
        curBulletCount = maxBublletCount;
    }
    public void UseBullet()
    {
        curBulletCount--; // ź �Һ�
    }
    public void ReloadBullet()
    {
        bulletshotCurTime = 0;
        curBulletCount = maxBublletCount;
    }
    /// <summary>
    /// ������ �Ծ��� �� ���ݾ�
    /// </summary>
    /// <param name="speed_up">�̵��ӵ�</param>
    /// <param name="jumppower_up">������</param>
    /// <param name="armorDurability_up">����</param>
    /// <param name="attackPower_up">���ݷ�</param>
    public void StatUP(float speed_up, float jumppower_up, int armorDurability_up, int attackPower_up)
    {
        speed += speed_up;
        JumpPoawer += jumppower_up;
        ArmorDurability += armorDurability_up;
        attackPower += attackPower_up;
    }
    #endregion
}

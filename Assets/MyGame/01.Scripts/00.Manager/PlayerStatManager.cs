using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;


public class PlayerStatManager : MonoBehaviour
{
    //public static PlayerStatManager instance;

    [Header("플레이어 움직임스텟")]
    public float speed;
    public float JumpPoawer;

    [Header("플레이어 체력스텟")]
    public int ArmorDurability = 0;

    [Header("플레이어 공격스텟")]
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
            Debug.Log("공격력 : " + attackPower + "이동속도 : " + speed);
        }
    }
    #endregion

    #region Private_Function
    //private void _OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    Debug.Log(scene.name);
    //    // 플레이어 스탯 초기화
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
        curBulletCount--; // 탄 소비
    }
    public void ReloadBullet()
    {
        bulletshotCurTime = 0;
        curBulletCount = maxBublletCount;
    }
    /// <summary>
    /// 아이템 먹었을 때 스텟업
    /// </summary>
    /// <param name="speed_up">이동속도</param>
    /// <param name="jumppower_up">점프력</param>
    /// <param name="armorDurability_up">방어력</param>
    /// <param name="attackPower_up">공격력</param>
    public void StatUP(float speed_up, float jumppower_up, int armorDurability_up, int attackPower_up)
    {
        speed += speed_up;
        JumpPoawer += jumppower_up;
        ArmorDurability += armorDurability_up;
        attackPower += attackPower_up;
    }
    #endregion
}

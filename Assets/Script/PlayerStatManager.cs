using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager instance;

    [Header("�÷��̾� �����ӽ���")]
    public float speed;
    public float JumpPoawer;

    [Header("�÷��̾� ü�½���")]
    public int MaxHp;
    public int CurHp;

    [Header("�÷��̾� ���ݽ���")]
    public int AttackPower = 1;
    public int CurBulletCount;
    public int MaxBublletCount;
    public float bulletshotCoolTime;
    public float bulletshotCurTime;
    

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
    }
    
}

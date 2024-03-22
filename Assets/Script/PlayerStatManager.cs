using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager instance;

    public int MaxHp;
    public int CurHp;
    public int AttackPower;
    public int CurBulletCount;
    public int MaxBublletCount;


    void Start()
    {
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
        
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MonsterStat
{
    public MonsterUnitCode unitCode { get; } // 바꿀 수 없게 get만
    public string name { get; set; } // 이름
    public int maxHp { get; set; } // 최대 체력
    public int curHp { get; set; } // 현재 체력
    public int atkDmg { get; set; } // 공격력
    public float atkSpeed { get; set; } // 공격속도
    public float curAtkSpeed { get; set; } // 현재 공격속도
    public float moveSpeed { get; set; } // 이동속도
    public float atkRange { get; set; } // 사거리 

    private Rigidbody2D rb;

    public MonsterStat()
    {
    }
    public MonsterStat(MonsterUnitCode unitCode, string name, int maxHp, int atkDmg, float atkSpeed, float moveSpeed, float atkRange)
    {
        this.unitCode = unitCode;
        this.name = name;
        this.maxHp = maxHp;
        curHp = maxHp;
        this.atkDmg = atkDmg;
        curAtkSpeed = atkSpeed;
        this.atkSpeed = atkSpeed;
        this.moveSpeed = moveSpeed;
        this.atkRange = atkRange;
    }
    public MonsterStat SetUnitStatus(MonsterUnitCode unitCode) // 사용법 : public Stat 변수 선언 -> stat = stat.SetUnitStatus(UnitCode.가져올 유닛의 이름)
    {                                                          // status = new MonsterStat(unitCode, "이름", 최대체력, 공격력, 공격속도, 이동속도, 사거리);
        MonsterStat status = null;

        switch (unitCode)
        {
            case MonsterUnitCode.Vampire:
                status = new MonsterStat(unitCode, "뱀파이어", 1, 10, 1f, 32f, 2); 
                break;
            case MonsterUnitCode.slime:
                status = new MonsterStat(unitCode, "슬라임", 100, 10, 1.5f, 2f, 1f);
                break;
            case MonsterUnitCode.pilwon:
                status = new MonsterStat(unitCode, "필원", 9999999, 999999, 999999f, 999999f, 999999f);
                break;
        }
        return status;
    }
}

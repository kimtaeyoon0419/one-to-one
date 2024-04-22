using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class MonsterStat
{
    public MonsterUnitCode unitCode { get; } // �ٲ� �� ���� get��
    public string name { get; set; } // �̸�
    public int maxHp { get; set; } // �ִ� ü��
    public int curHp { get; set; } // ���� ü��
    public int atkDmg { get; set; } // ���ݷ�
    public float atkSpeed { get; set; } // ���ݼӵ�
    public float curAtkSpeed { get; set; } // ���� ���ݼӵ�
    public float moveSpeed { get; set; } // �̵��ӵ�
    public float atkRange { get; set; } // ��Ÿ� 

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
    public MonsterStat SetUnitStatus(MonsterUnitCode unitCode) // ���� : public Stat ���� ���� -> stat = stat.SetUnitStatus(UnitCode.������ ������ �̸�)
    {                                                          // status = new MonsterStat(unitCode, "�̸�", �ִ�ü��, ���ݷ�, ���ݼӵ�, �̵��ӵ�, ��Ÿ�);
        MonsterStat status = null;

        switch (unitCode)
        {
            case MonsterUnitCode.Vampire:
                status = new MonsterStat(unitCode, "�����̾�", 1, 10, 1f, 32f, 2); 
                break;
            case MonsterUnitCode.slime:
                status = new MonsterStat(unitCode, "������", 100, 10, 1.5f, 2f, 1f);
                break;
            case MonsterUnitCode.pilwon:
                status = new MonsterStat(unitCode, "�ʿ�", 9999999, 999999, 999999f, 999999f, 999999f);
                break;
        }
        return status;
    }
}

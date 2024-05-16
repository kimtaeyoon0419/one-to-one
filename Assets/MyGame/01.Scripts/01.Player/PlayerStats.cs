// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("StatDB")]
    [SerializeField] private int branch;
    [SerializeField] private StatsDB statDB;

    [Header("�÷��̾� ������")]
    public string charName;

    [Header("�÷��̾� �����ӽ���")]
    public float speed;
    public float JumpPoawer;

    [Header("�÷��̾� ü�½���")]
    public int ArmorDurability = 0;

    [Header("�÷��̾� ���ݽ���")]
    public int attackPower = 1;

    private void Awake()
    {
        for (int i = 0; i < statDB.Stats.Count; i++)
        {
            if (statDB.Stats[i].branch == branch)
            {
                charName = statDB.Stats[i].name;
                speed = statDB.Stats[i].speed;                                                          // �̵��ӵ� �ʱ�ȭ
                JumpPoawer = statDB.Stats[i].jumppower;                                    // ������ �ʱ�ȭ
                ArmorDurability = statDB.Stats[i].armordurability;                        // ���� �ʱ�ȭ
                attackPower = statDB.Stats[i].attackpower;                                // ���ݷ� �ʱ�ȭ
            }
        }
    }
}

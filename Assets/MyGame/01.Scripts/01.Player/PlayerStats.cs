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
    public int ArmorDurability;

    [Header("�÷��̾� ���ݽ���")]
    public static int attackPower;

    #region Unity_Funtion
    private void Awake()
    {
        GetStat();
    }

    private void OnDisable()
    {
        SaveStat();
    }
    #endregion

    #region Private_Function
    private void GetStat()
    {
        branch = GameManager.instance.selectChar;
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

    private void SaveStat()
    {
        for (int i = 0; i < statDB.Stats.Count; i++)
        {
            if (statDB.Stats[i].branch == branch)
            {
                statDB.Stats[i].name = charName;
                statDB.Stats[i].speed = speed;                                                          // �̵��ӵ� �ʱ�ȭ
                statDB.Stats[i].jumppower = JumpPoawer;                                    // ������ �ʱ�ȭ
                statDB.Stats[i].armordurability = ArmorDurability;                        // ���� �ʱ�ȭ
                statDB.Stats[i].attackpower = attackPower;                                // ���ݷ� �ʱ�ȭ
            }
        }
    }
    #endregion
}

// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("StatDB")]
    [SerializeField] private int branch; // ĳ���� ��ȣ
    [SerializeField] private StatsDB statDB; // ���� �����ͺ��̽�

    [Header("�÷��̾� ������")]
    public string charName; // ĳ���� �̸�

    [Header("�÷��̾� �����ӽ���")]
    public float speed; // ĳ���� �̵��ӵ�
    public float JumpPoawer; // ĳ���� ������

    [Header("�÷��̾� ü�½���")]
    public int ArmorDurability; // ĳ���� ����

    [Header("�÷��̾� ���ݽ���")]
    public static int attackPower; // ĳ���� ���ݷ�

    #region Unity_Funtion
    private void Awake()
    {
        GetStat(); // ó�� ������ �� ���� �ҷ�����
    }

    private void OnDisable()
    {
        SaveStat(); // �� �Ѿ�Ű� �� ���� ����
    }
    #endregion

    #region Private_Function
    private void GetStat() // �������� ���� �ҷ�����
    {
        branch = GameManager.instance.selectChar;
        for (int i = 0; i < statDB.Stats.Count; i++)
        {
            if (statDB.Stats[i].branch == branch)
            {
                charName = statDB.Stats[i].name;                                                   // ĳ���� �̸� �ʱ�ȭ
                speed = statDB.Stats[i].speed;                                                         // �̵��ӵ� �ʱ�ȭ
                JumpPoawer = statDB.Stats[i].jumppower;                                    // ������ �ʱ�ȭ
                ArmorDurability = statDB.Stats[i].armordurability;                        // ���� �ʱ�ȭ
                attackPower = statDB.Stats[i].attackpower;                                // ���ݷ� �ʱ�ȭ
            }
        }
    }

    private void SaveStat() // ������ ��������
    {
        for (int i = 0; i < statDB.Stats.Count; i++)
        {
            if (statDB.Stats[i].branch == branch)
            {
                statDB.Stats[i].name = charName;                                                   // ���� ĳ���� �̸� �ʱ�ȭ
                statDB.Stats[i].speed = speed;                                                          // ���� �̵��ӵ� �ʱ�ȭ
                statDB.Stats[i].jumppower = JumpPoawer;                                    // ���� ������ �ʱ�ȭ
                statDB.Stats[i].armordurability = ArmorDurability;                         // ���� ���� �ʱ�ȭ
                statDB.Stats[i].attackpower = attackPower;                                // ���� ���ݷ� �ʱ�ȭ
            }
        }
    }
    #endregion
}

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
    [SerializeField] private bool stage_1;
    [SerializeField] private int saveBranch = 4;

    [Header("�÷��̾� ������")]
    public string charName; // ĳ���� �̸�

    [Header("�÷��̾� �����ӽ���")]
    public float speed; // ĳ���� �̵��ӵ�
    public float jumpPoawer; // ĳ���� ������

    [Header("�÷��̾� ü�½���")]
    public int armorDurability; // ĳ���� ����

    [Header("�÷��̾� ���ݽ���")]
    public static int attackPower; // ĳ���� ���ݷ�

    #region Unity_Funtion
    private void Start()
    {
        if (stage_1 == true)
        {
            GetStat(); // ó�� ����������� ������ ĳ���� ���� �ҷ�����
            stage_1 = false;    
        }
        else SaveStatLoad();
    }

    private void OnDisable()
    {
        //if (stage_1 == false)
        //{
            Debug.Log("������ ����Ǿ����ϴ�");
            if (GameManager.instance.curGameState != CurGameState.gameOver)
                SaveStat(); // �� �Ѿ�Ű� �� ���� ����
        //}
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
                jumpPoawer = statDB.Stats[i].jumppower;                                    // ������ �ʱ�ȭ
                armorDurability = statDB.Stats[i].armordurability;                        // ���� �ʱ�ȭ
                attackPower = statDB.Stats[i].attackpower;                                // ���ݷ� �ʱ�ȭ
            }
        }
    }

    private void SaveStatLoad()
    {
        for (int i = 0; i < statDB.Stats.Count; i++)
        {
            if (statDB.Stats[i].branch == 4)
            {
                charName = statDB.Stats[i].name;                                                   // ������ ĳ���� �̸� �ҷ�����
                speed = statDB.Stats[i].speed;                                                         // ������ �̵��ӵ� �ҷ�����
                jumpPoawer = statDB.Stats[i].jumppower;                                    // ������ ������ �ҷ�����
                armorDurability = statDB.Stats[i].armordurability;                        // ������ ���� �ҷ�����
                attackPower = statDB.Stats[i].attackpower;                                // ������ ���ݷ� �ҷ�����
            }
        }
    }

    private void SaveStat() // ������ ��������
    {
        for (int i = 0; i < statDB.Stats.Count; i++)
        {
            if (statDB.Stats[i].branch == 4)
            {
                statDB.Stats[i].name = charName;                                                  // ���� ĳ���� �̸� �ʱ�ȭ
                statDB.Stats[i].speed = speed;                                                         // ���� �̵��ӵ� �ʱ�ȭ
                statDB.Stats[i].jumppower = jumpPoawer;                                    // ���� ������ �ʱ�ȭ
                statDB.Stats[i].armordurability = armorDurability;                        // ���� ���� �ʱ�ȭ
                statDB.Stats[i].attackpower = attackPower;                               // ���� ���ݷ� �ʱ�ȭ
            }
        }
    }
    #endregion
}

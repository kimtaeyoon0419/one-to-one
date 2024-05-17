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

    [Header("플레이어 움직임")]
    public string charName;

    [Header("플레이어 움직임스텟")]
    public float speed;
    public float JumpPoawer;

    [Header("플레이어 체력스텟")]
    public int ArmorDurability;

    [Header("플레이어 공격스텟")]
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
                speed = statDB.Stats[i].speed;                                                          // 이동속도 초기화
                JumpPoawer = statDB.Stats[i].jumppower;                                    // 점프력 초기화
                ArmorDurability = statDB.Stats[i].armordurability;                        // 방어력 초기화
                attackPower = statDB.Stats[i].attackpower;                                // 공격력 초기화
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
                statDB.Stats[i].speed = speed;                                                          // 이동속도 초기화
                statDB.Stats[i].jumppower = JumpPoawer;                                    // 점프력 초기화
                statDB.Stats[i].armordurability = ArmorDurability;                        // 방어력 초기화
                statDB.Stats[i].attackpower = attackPower;                                // 공격력 초기화
            }
        }
    }
    #endregion
}

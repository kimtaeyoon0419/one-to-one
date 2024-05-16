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
    public int ArmorDurability = 0;

    [Header("플레이어 공격스텟")]
    public int attackPower = 1;

    private void Awake()
    {
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
}

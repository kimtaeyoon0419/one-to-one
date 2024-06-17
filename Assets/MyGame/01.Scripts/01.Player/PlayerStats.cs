// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("StatDB")]
    [SerializeField] private int branch; // 캐릭터 번호
    [SerializeField] private StatsDB statDB; // 스텟 데이터베이스
    [SerializeField] private bool stage_1;
    [SerializeField] private int saveBranch = 4;

    [Header("플레이어 움직임")]
    public string charName; // 캐릭터 이름

    [Header("플레이어 움직임스텟")]
    public float speed; // 캐릭터 이동속도
    public float jumpPoawer; // 캐릭터 점프력

    [Header("플레이어 체력스텟")]
    public int armorDurability; // 캐릭터 방어력

    [Header("플레이어 공격스텟")]
    public static int attackPower; // 캐릭터 공격력

    #region Unity_Funtion
    private void Start()
    {
        if (stage_1 == true)
        {
            GetStat(); // 처음 스테이지라면 선택한 캐릭터 스텟 불러오기
            stage_1 = false;    
        }
        else SaveStatLoad();
    }

    private void OnDisable()
    {
        //if (stage_1 == false)
        //{
            Debug.Log("스텟이 저장되었습니다");
            if (GameManager.instance.curGameState != CurGameState.gameOver)
                SaveStat(); // 씬 넘어거갈 때 스텟 저장
        //}
    }
    #endregion

    #region Private_Function
    private void GetStat() // 엑셀에서 스텟 불러오기
    {
        branch = GameManager.instance.selectChar;
        for (int i = 0; i < statDB.Stats.Count; i++)
        {
            if (statDB.Stats[i].branch == branch)
            {
                charName = statDB.Stats[i].name;                                                   // 캐릭터 이름 초기화
                speed = statDB.Stats[i].speed;                                                         // 이동속도 초기화
                jumpPoawer = statDB.Stats[i].jumppower;                                    // 점프력 초기화
                armorDurability = statDB.Stats[i].armordurability;                        // 방어력 초기화
                attackPower = statDB.Stats[i].attackpower;                                // 공격력 초기화
            }
        }
    }

    private void SaveStatLoad()
    {
        for (int i = 0; i < statDB.Stats.Count; i++)
        {
            if (statDB.Stats[i].branch == 4)
            {
                charName = statDB.Stats[i].name;                                                   // 저장한 캐릭터 이름 불러오기
                speed = statDB.Stats[i].speed;                                                         // 저장한 이동속도 불러오기
                jumpPoawer = statDB.Stats[i].jumppower;                                    // 저장한 점프력 불러오기
                armorDurability = statDB.Stats[i].armordurability;                        // 저장한 방어력 불러오기
                attackPower = statDB.Stats[i].attackpower;                                // 저장한 공격력 불러오기
            }
        }
    }

    private void SaveStat() // 엑셀에 스텟저장
    {
        for (int i = 0; i < statDB.Stats.Count; i++)
        {
            if (statDB.Stats[i].branch == 4)
            {
                statDB.Stats[i].name = charName;                                                  // 엑셀 캐릭터 이름 초기화
                statDB.Stats[i].speed = speed;                                                         // 엑셀 이동속도 초기화
                statDB.Stats[i].jumppower = jumpPoawer;                                    // 엑셀 점프력 초기화
                statDB.Stats[i].armordurability = armorDurability;                        // 엑셀 방어력 초기화
                statDB.Stats[i].attackpower = attackPower;                               // 엑셀 공격력 초기화
            }
        }
    }
    #endregion
}

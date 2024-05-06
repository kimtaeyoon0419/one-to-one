using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class test_PlayerStat : MonoBehaviour
{
    public PlayerCharCode character;
    public int durability;
    public int damage;
    public int attack_speed;
    public float movespeed;
    public float jummpower;
    #region Public_Function
    public test_PlayerStat()
    {

    }
    /// <summary>
    /// 플레이어 스텟 초기화
    /// </summary>
    /// <param name="durability">방어력</param>
    /// <param name="damage">공격력</param>
    /// <param name="attack_speed">공격속도</param>
    /// <param name="movespeed">이동속도</param>
    /// <param name="jummpower">점프파워</param>
    public test_PlayerStat(PlayerCharCode charater, int durability, int damage, int attack_speed, float movespeed, float jummpower)
    {
        this.durability = durability;
        this.damage = damage;
        this.attack_speed = attack_speed;
        this.movespeed = movespeed;
        this.jummpower = jummpower;
    }

    /// <summary>
    /// 유닛코드로 캐릭터마다 스텟지정
    /// </summary>
    /// <param name="charater">선택한 캐릭터(유닛코드)</param>
    /// <returns></returns>
    public test_PlayerStat CharSlelectStatSet(PlayerCharCode charater)
    {
        test_PlayerStat stats = null;

        switch (charater)
        {
            case PlayerCharCode.cat:
                stats = new test_PlayerStat(charater, 0, 10, 1, 7, 12);
                break;
            case PlayerCharCode.dog:
                stats = new test_PlayerStat(charater, 0, 15, 1, 5, 7);
                break;
            case PlayerCharCode.knight:
                stats = new test_PlayerStat(charater, 0, 5, 1, 5, 8);
                break;
        }
        return stats;
    }

    /// <summary>
    /// 클리어 후 아이템 먹고 스텟 증가
    /// </summary>
    /// <param name="up_durability">방어력</param>
    /// <param name="up_damage">공격력</param>
    /// <param name="up_attack_speed">공격속도</param>
    /// <param name="up_movespeed">이동속도</param>
    /// <param name="up_jummpower">점프파워</param>
    public void CharaterStatUp(int up_durability, int up_damage, int up_attack_speed, float up_movespeed, float up_jummpower)
    {
        durability += up_durability;
        damage += up_damage;
        attack_speed += up_attack_speed;
        movespeed += up_movespeed;
        jummpower += up_jummpower;
    }
    #endregion
}

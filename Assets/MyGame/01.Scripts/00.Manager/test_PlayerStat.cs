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
    /// �÷��̾� ���� �ʱ�ȭ
    /// </summary>
    /// <param name="durability">����</param>
    /// <param name="damage">���ݷ�</param>
    /// <param name="attack_speed">���ݼӵ�</param>
    /// <param name="movespeed">�̵��ӵ�</param>
    /// <param name="jummpower">�����Ŀ�</param>
    public test_PlayerStat(PlayerCharCode charater, int durability, int damage, int attack_speed, float movespeed, float jummpower)
    {
        this.durability = durability;
        this.damage = damage;
        this.attack_speed = attack_speed;
        this.movespeed = movespeed;
        this.jummpower = jummpower;
    }

    /// <summary>
    /// �����ڵ�� ĳ���͸��� ��������
    /// </summary>
    /// <param name="charater">������ ĳ����(�����ڵ�)</param>
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
    /// Ŭ���� �� ������ �԰� ���� ����
    /// </summary>
    /// <param name="up_durability">����</param>
    /// <param name="up_damage">���ݷ�</param>
    /// <param name="up_attack_speed">���ݼӵ�</param>
    /// <param name="up_movespeed">�̵��ӵ�</param>
    /// <param name="up_jummpower">�����Ŀ�</param>
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

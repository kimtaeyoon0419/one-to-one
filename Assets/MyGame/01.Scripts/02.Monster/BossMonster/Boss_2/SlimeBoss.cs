// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class SlimeBoss : BossMonster
{
    [Header("SlimeStat")]
    [SerializeField] private float jumpPower;

    #region ����
    protected override IEnumerator UseSkill(int skillNum)
    {
        if(skillNum == 0) // �뽬
        {
            animator.SetTrigger(hashSkill1);
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            //rigid.AddForce(new Vector2())

        }

        else if(skillNum == 1) // ����
        {
            animator.SetTrigger(hashSkill2);
        }

        yield return null;
    }

    #endregion


    protected override void Die()
    {
        
    }
}

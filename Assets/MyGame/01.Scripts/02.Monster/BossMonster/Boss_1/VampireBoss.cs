// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class VampireBoss : BossMonster
{
    [Header("SkillPos")]
    [SerializeField] private Transform useMoveSkillPos;
    [SerializeField] private Transform useShotSkillPos;

    [Header("SkillObject")]
    [SerializeField] private GameManager slashEffect;
    [SerializeField] private GameManager firePillar;

    [Header("Animation")]
    ///<summary>
    /// 긁기 애니메이션
    ///</summary>
    protected readonly int hashSkill1 = Animator.StringToHash("Skill1");
    /// <summary>
    /// 가스 뿜기 애니메이션
    /// </summary>
    protected readonly int hashSkill2 = Animator.StringToHash("Skill2");
    /// <summary>
    /// 휴식 애니메이션?
    /// </summary>
    protected readonly int hashSkill3 = Animator.StringToHash("Skill3");

    protected override IEnumerator UseSkill(int skillNum)
    {
        if(skillNum == 0)
        {
            animator.SetTrigger(hashSkill1);
            Instantiate(slashEffect, useShotSkillPos.position, Quaternion.Euler(0, transform.rotation.y, 0));
        }
        else if (skillIndex == 1)
        {
            animator.SetTrigger(hashSkill2);
            Instantiate(firePillar, player.transform.position, Quaternion.identity);
        }
        else if(skillIndex == 2)
        {
            animator.SetTrigger(hashSkill3);
            transform.position = player.transform.position;
        }

        state = BossState.fight;
        StartCoroutine(SkillTriger(skillTrigerTime));
        yield return null;
    }

    protected override IEnumerator SkillTriger(float time)
    {
        throw new System.NotImplementedException();
    }

    protected override void Die()
    {
        
    }
}

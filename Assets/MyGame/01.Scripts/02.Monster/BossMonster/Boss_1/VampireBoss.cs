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
    [SerializeField] private GameObject slashEffect;
    [SerializeField] private GameObject firePillar;

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

    protected override IEnumerator UseSkill()
    {
        state = BossState.usingSkill;
        int skillNum = Random.Range(0, 2);
        Debug.Log(skillNum);
        if(skillNum == 0)
        {
            Instantiate(slashEffect, useShotSkillPos.position, Quaternion.Euler(0, 180 *gameObject.transform.rotation.y, 0));
            animator.SetTrigger(hashSkill1);
        }
        if (skillNum == 1)
        {
            yield return StartCoroutine(FirePillarAttack());
        }
        //if(skillIndex == 2)
        //{
        //    animator.SetTrigger(hashSkill3);
        //    transform.position = player.transform.position;
        //}

        state = BossState.fight;
        StartCoroutine(SkillTriger(skillTrigerTime));
        yield return null;
    }

    private IEnumerator FirePillarAttack()
    {
        Debug.Log("불기둥 스킬");
        for (int i = 0; i < 3; i++)
        {
            animator.SetTrigger(hashSkill2);
            yield return new WaitForSeconds(0.1f);
            yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        }
        animator.SetTrigger(hashSkill3);
    }


    public void FireSkill()
    {
        Instantiate(firePillar, player.transform.position, Quaternion.identity);
    }
    
    protected override IEnumerator SkillTriger(float time)
    {
        yield return new WaitForSeconds(time);
        state = BossState.useSkill;
    }

    protected override void Die()
    {
        
    }
}

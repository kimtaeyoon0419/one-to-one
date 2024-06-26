// # System
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


// # Unity
using UnityEngine;

public class VampireBoss : BossMonster
{
    [Header("SkillPos")]
    [SerializeField] private Transform useMoveSkillPos;
    [SerializeField] private Transform useShotSkillPos;

    [Header("SkillObject")]
    [SerializeField] private GameObject[] slashEffect;
    [SerializeField] private GameObject firePillar;
    [SerializeField] private GameObject smokeEffect;
    [SerializeField] private GameObject alterEgo;

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
    /// <summary>
    /// 은신 애니메이션
    /// </summary>
    protected readonly int hashSkill4 = Animator.StringToHash("Skill4");

    [Header("Move")]
    [SerializeField] private float maxMoveTime = 5f;
    [SerializeField] private float curMoveTime = 0f;

    [Header("MoveTransform")]
    [SerializeField] private Transform[] movePoss;

    [Header("alterEgoSkill")]
    [SerializeField] private List<GameObject> alteregos = new List<GameObject>();
    [SerializeField] private bool findFact = false;

    /// <summary>
    /// 스킬 실행해주는 스크립트
    /// </summary>
    /// <returns></returns>
    protected override IEnumerator UseSkill()
    {
        state = BossState.usingSkill;
        int skillNum = Random.Range(0, 3);
        Debug.Log(skillNum);
        if(skillNum == 0)
        {
            int index = Random.Range(0, slashEffect.Length);
            AudioManager.instance.PlaySFX("VampireSlash");
            Instantiate(slashEffect[index], useShotSkillPos.position, Quaternion.Euler(0, 180 *gameObject.transform.rotation.y, 0));
            animator.SetTrigger(hashSkill1);
        }
        if (skillNum == 1)
        {
            yield return StartCoroutine(FirePillarAttack());
        }
        if (skillNum == 2)
        {
            yield return StartCoroutine(StealthAttack());
        }
        state = BossState.fight;
        StartCoroutine(SkillTriger(skillTrigerTime));
        yield return null;
    }

    /// <summary>
    /// 정해진 위치로 랜덤으로 이동
    /// </summary>
    protected override void Move()
    {
        if(curMoveTime <= 0)
        {
            int movePos = Random.Range(0, movePoss.Length);

            curMoveTime = maxMoveTime;
            Instantiate(smokeEffect, transform.position, Quaternion.identity);
            transform.position = movePoss[movePos].position;
        }
        else
        {
            curMoveTime -= Time.deltaTime;
        }
    }

    #region Skill
    /// <summary>
    /// 불기둥 공격 총 3번 공격 후 휴식
    /// </summary>
    /// <returns></returns>
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
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
    }

    /// <summary>
    /// 은신 공격 총 6마리로 나눠지며 진짜 찾기
    /// </summary>
    /// <returns></returns>
    private IEnumerator StealthAttack()
    {
        yield return null;
        int tr = Random.Range(0, movePoss.Length);
        findFact = false;
        animator.SetBool(hashSkill4, true);
        transform.position = movePoss[tr].position;
        float originalHP = curHp;
        

        for(int i = 0; i < movePoss.Length; i++)
        {
            if (i != tr)
            {
                GameObject egos = Instantiate(alterEgo, movePoss[i].position, Quaternion.identity);
                alteregos.Add(egos);
            }
        }

        yield return null;

        while (!findFact)
        {
            for (int i = alteregos.Count - 1; i >= 0; i--)
            {
                if (!alteregos[i].activeSelf)
                {
                    GameObject toRemove = alteregos[i];
                    alteregos.RemoveAt(i);
                    Destroy(toRemove);
                }
            }

            yield return null;

            if (alteregos.Count <= 0 || originalHP > curHp)
            {
                findFact = true;
            }
        }

        if (findFact)
        {
            animator.SetBool(hashSkill4, false);
            if (alteregos.Count > 0)
            {
                for (int i = alteregos.Count - 1; i >= 0; i--)
                {
                    GameObject toRemove = alteregos[i];
                    alteregos.RemoveAt(i);
                    Destroy(toRemove);
                }
                alteregos.Clear();
            }
        }

        animator.SetTrigger(hashSkill3);
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
    }

    /// <summary>
    /// 애니메이터에서 이벤트로 실행할 불기둥 생성 함수
    /// </summary>
    public void FireSkill()
    {
        Instantiate(firePillar, player.transform.position, Quaternion.identity);
    }
    /// <summary>
    /// 일정 시간 뒤에 스킬 발동
    /// </summary>
    /// <param name="time">기다릴 시간</param>
    /// <returns></returns>
    protected override IEnumerator SkillTriger(float time)
    {
        yield return new WaitForSeconds(time);
        state = BossState.useSkill;
    }
    #endregion

    #region 피격 처리
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (!findFact)
    //    {
    //        if(collision.gameObject.CompareTag("Player"))
    //        {
    //            findFact = true;
    //        }
    //    }
    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!findFact)
    //    {
    //        if (collision.gameObject.CompareTag("Bullet"))
    //        {
    //            findFact = true;
    //        }
    //    }
    //}
    #endregion
}

// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class SlimeBoss : BossMonster
{
    [Header("SlimeStat")]
    [SerializeField] private float dashDistance;
    [SerializeField] private float dashTime;
    [SerializeField] private bool isDash;
    [SerializeField] private float jumpPower;
    [SerializeField] private bool isJump;
    [SerializeField] private Vector3 initialScale;
    [SerializeField] private Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);

    [Header("Animation")]
    /// <summary>
    /// 대쉬 애니메이션
    /// </summary>
    protected readonly int hashSkill1 = Animator.StringToHash("Skill1");
    /// <summary>
    /// 점프 애니메이션
    /// </summary>
    protected readonly int hashSkill2 = Animator.StringToHash("Skill2");
    /// <summary>
    /// 공중에서 낙하하는 애니메이션
    /// </summary>
    private readonly int hashDown = Animator.StringToHash("Down");

    protected override void Start()
    {
        base.Start();
        initialScale = transform.localScale;
        Debug.Log(initialScale);
    }

    protected override void Update()
    {
        base.Update();
        if (curHp < 50f)
        {
            skillTrigerTime = 0.2f;
        }
        SizeDown();
    }


    #region 공격
    protected override IEnumerator UseSkill()
    {
        int skillNum = Random.Range(0, 10);

        if (skillNum >= 0 &&  skillNum <= 6) // 대쉬
        {
            animator.SetTrigger(hashSkill1);
            isDash = true;
            rigid.velocity = new Vector2(rigid.velocity.x, 0f);
            rigid.AddForce(new Vector2(dashDistance * isRight, 0f), ForceMode2D.Impulse);
            float gravity = rigid.gravityScale;
            rigid.gravityScale = 0f;
            yield return new WaitForSeconds(dashTime);
            isDash = false;
            rigid.gravityScale = gravity;
        }

        else if (skillNum >= 7 && skillNum <= 9) // 점프
        {
            animator.SetTrigger(hashSkill2);
            isJump = true;
            Move();
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            //rigid.velocity = Vector2.right * jumpPower;
            //rigid.velocity = Vector2.up * jumpPower;
            while (isJump)
            {
                animator.SetBool(hashDown, true);
                if (!isJump)
                {
                    break;
                }
                yield return null;
            }
        }
        state = BossState.fight;
        animator.SetBool(hashDown, false);
        StartCoroutine(SkillTriger(skillTrigerTime));
        yield return null;
    }

    protected override IEnumerator SkillTriger(float time)
    {
        yield return new WaitForSeconds(time);
        if (state == BossState.usingSkill)
        {
            StartCoroutine(SkillTriger(skillTrigerTime));
        }
        else
        {
            state = BossState.useSkill;
        }
    }

    #endregion

    protected override void Die()
    {

    }

    private void SizeDown()
    {
        float healthRatio = (float)curHp / maxHp;
        Debug.Log("현재 체력 / 최대 체력 : " + healthRatio);

        Vector3 newScale = initialScale * healthRatio;

        newScale.x = Mathf.Clamp(newScale.x, minScale.x, initialScale.x);
        newScale.y = Mathf.Clamp(newScale.y, minScale.y, initialScale.y);
        newScale.z = Mathf.Clamp(newScale.z, minScale.z, initialScale.z);
        Debug.Log(newScale);

        transform.localScale = newScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isJump && collision.gameObject.CompareTag("Ground"))
        {
            isJump = false;
        }
    }
}

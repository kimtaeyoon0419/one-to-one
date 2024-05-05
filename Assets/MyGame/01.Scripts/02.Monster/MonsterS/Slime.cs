using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : Monster
{
    public float JumpScale;
    private bool isGround = false;
    float rayLookDir; // 몬스터의 방향과 맞는 레이 방향

    #region Unity_Function
    protected override void Update()
    {
        base.Update();
        AttackRay();
    }

    /// <summary>
    /// 바닥에 있는지 확인
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // 현재 땅에 붙어있는지 확인
        {
            isGround = true;
        }
    }
    #endregion

    #region Private_Function
    /// <summary>
    /// 플레이어가 있는지 검사하는 레이케이스
    /// </summary>
    private void AttackRay() //레이케스트에 플레이어가 들어오면 공격!!!
    {
        if (nextMove != 0)
        {
            rayLookDir = nextMove * 0.6f;
        }
        Debug.DrawRay(rb.position, Vector2.right * stat.atkRange * rayLookDir, Color.yellow); // 레이확인
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right * rayLookDir, stat.atkRange, LayerMask.GetMask("Player"));

        if (hit.collider != null && stat.curAtkSpeed <= 0)
        {
            stat.curAtkSpeed = stat.atkSpeed;
            StopCoroutine(Co_Think()); // 코루틴을 꺼서 방향 전환을 막음
            isAttack = true;
            Attack(); // 공격 후
            if (isAttack == false)
            {
                StartCoroutine(Co_StartThinkCoroutineDelay(2f)); // 다시 방향전환을 정함
            }
        }
    }


    private void Attack()
    {
        if (isGround)
        {
            Debug.Log("홀리씨우");
            isGround = false;
            animator.SetTrigger("Attack"); // 공격 애니메이션
            AudioManager.instance.PlaySFX("Slime_Jump");
            rb.velocity = Vector2.up * JumpScale;
        }
        isAttack = false;
    }
    #endregion
}


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
    float rayLookDir; // ������ ����� �´� ���� ����

    #region Unity_Function
    protected override void Update()
    {
        base.Update();
        AttackRay();
    }

    /// <summary>
    /// �ٴڿ� �ִ��� Ȯ��
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // ���� ���� �پ��ִ��� Ȯ��
        {
            isGround = true;
        }
    }
    #endregion

    #region Private_Function
    /// <summary>
    /// �÷��̾ �ִ��� �˻��ϴ� �������̽�
    /// </summary>
    private void AttackRay() //�����ɽ�Ʈ�� �÷��̾ ������ ����!!!
    {
        if (nextMove != 0)
        {
            rayLookDir = nextMove * 0.6f;
        }
        Debug.DrawRay(rb.position, Vector2.right * stat.atkRange * rayLookDir, Color.yellow); // ����Ȯ��
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right * rayLookDir, stat.atkRange, LayerMask.GetMask("Player"));

        if (hit.collider != null && stat.curAtkSpeed <= 0)
        {
            stat.curAtkSpeed = stat.atkSpeed;
            StopCoroutine(Co_Think()); // �ڷ�ƾ�� ���� ���� ��ȯ�� ����
            isAttack = true;
            Attack(); // ���� ��
            if (isAttack == false)
            {
                StartCoroutine(Co_StartThinkCoroutineDelay(2f)); // �ٽ� ������ȯ�� ����
            }
        }
    }


    private void Attack()
    {
        if (isGround)
        {
            Debug.Log("Ȧ������");
            isGround = false;
            animator.SetTrigger("Attack"); // ���� �ִϸ��̼�
            AudioManager.instance.PlaySFX("Slime_Jump");
            rb.velocity = Vector2.up * JumpScale;
        }
        isAttack = false;
    }
    #endregion
}


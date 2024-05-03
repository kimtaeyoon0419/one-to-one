using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : Monster
{
    private Vector2 JumpPower = new Vector2(2f, 4f);  // ���� ���� �����Ŀ�
    private float JumpScale = 2f;

    #region Unity_Function
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void Update()
    {
        base.Update();
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

    #region override_Function
    protected override void Attack()
    {   if (isGround)
        {
            base.Attack();
            AudioManager.instance.PlaySFX("Slime_Jump");
            //rb.velocity = JumpPower; // velocity ���� JumpPower�� �ʱ�ȭ�ϰ�
            //rb.velocity = new Vector2(rb.velocity.x * stat.moveSpeed * rayLookDir, rb.velocity.y); // ������ �������� ���� �Ѵ�!
            rb.AddForce(Vector2.up * JumpScale, ForceMode2D.Impulse);
        }
    }
    #endregion
}


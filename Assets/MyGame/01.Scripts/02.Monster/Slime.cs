using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : Monster
{
    private Vector2 JumpPower = new Vector2(2f, 4f);  // 점프 공격 점프파워
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

    #region override_Function
    protected override void Attack()
    {   if (isGround)
        {
            base.Attack();
            AudioManager.instance.PlaySFX("Slime_Jump");
            //rb.velocity = JumpPower; // velocity 값을 JumpPower로 초기화하고
            //rb.velocity = new Vector2(rb.velocity.x * stat.moveSpeed * rayLookDir, rb.velocity.y); // 레이의 방향으로 점프 한다!
            rb.AddForce(Vector2.up * JumpScale, ForceMode2D.Impulse);
        }
    }
    #endregion
}


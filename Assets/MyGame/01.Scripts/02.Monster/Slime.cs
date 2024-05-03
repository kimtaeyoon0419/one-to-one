using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : Monster
{
    public float JumpScale ;

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

    #region override_Function
    protected override void Attack()
    {   
            Debug.Log("Ȧ������");
            isGround = false;
            AudioManager.instance.PlaySFX("Slime_Jump");
            rb.velocity = Vector2.up * JumpScale;
    }
    #endregion
}


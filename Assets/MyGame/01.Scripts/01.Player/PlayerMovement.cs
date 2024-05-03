using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerMovement : MonoBehaviour
{
    [Header("Rigidbody")]
    [SerializeField] private Rigidbody2D rb;

    [Header("LayCast")]
    [SerializeField] private Transform groundChk;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallChk;
    [SerializeField] private LayerMask wallLayer;

    [Header("WallJump")]
    private bool iswallSliding; // ���� ���� Ÿ�� �ִ���
    private bool isWallJumping; // ���� ������ ������
    private float wallSlidingSpeed = 2f; // �� Ÿ�� �������� �ӵ�
    private float wallJumpingTime = 0.2f; // ������ ������ ���� �������� ������ �ð�
    private float wallJumpingCounter; // �������� ������ �ð�
    private float wallJumpingDirection; // ������ ����
    private float wallJumpingDuration = 0.2f; // ������ ���ӽð�
    private Vector2 wallJumpingPower = new Vector2(4f, 10f); // ������ ����  

    [Header("Move")]
    private float hor; // hor = Input.GetAxis("Horizontal"); �뵵
    private float isFacingRight = 1; // Flip �뵵
    private bool isGround = false;
    private Vector2 velocity;

    [Header("Coroutine")]
    private Coroutine Co_StopWallJumping;

    #region Unity_Function
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        hor = Input.GetAxis("Horizontal");

        _Jump();
        _wallSlide();
        _WallJump();
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            _Move();
            _Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // �ٴڿ� �ִ��� üũ
        {
            isGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision) // �ٴڿ� ���������� üũ
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGround = false;
        }
    }
    #endregion

    #region Private_Function
    /// <summary>
    /// ������
    /// </summary>
    private void _Move() // �÷��̾� ������
    { 
        velocity.x = hor * PlayerStatManager.instance.speed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity; // new�� �����ϱ� ���� Vector2 velocity ���� �� �ʱ�ȭ
    }

    /// <summary>
    /// ����
    /// </summary>
    private void _Jump()
    {
        if (Input.GetButtonDown("Jump") && isGround)
        {
            //rb.velocity = new Vector2(rb.velocity.x, PlayerStatManager.instance.JumpPoawer);
            rb.AddForce(Vector2.up * PlayerStatManager.instance.JumpPoawer, ForceMode2D.Impulse);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    /// <summary>
    /// ���� �پ��ִ��� üũ
    /// </summary>
    /// <returns></returns>
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallChk.position, 0.2f, wallLayer);
        // ���� wallChk.position�� 0.2f ũ���� �� �ȿ� wallLayer�� �ε����ٸ� true�� ����
    }

    /// <summary>
    /// �� �����̵�
    /// </summary>
    private void _wallSlide() // �� �����̵�
    {
        if (IsWalled() && !isGround && hor != 0)
        {
            iswallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            // velocity�� y���� wallSlidingSpeed��ŭ �������� ���� �տ� - �� ����
        }
        else
        {
            iswallSliding = false;
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    private void _WallJump()
    {
        if (iswallSliding) // �������� ������ ����
        {
            isWallJumping = false; // ������ Ȱ��ȭ
            wallJumpingCounter = wallJumpingTime; // ������ ������ �� �ִ� �ð�

            if(Co_StopWallJumping != null) StopCoroutine(Co_StopWallJumping);
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            float jumpDirection = isFacingRight; // ���� ���� ����
            isWallJumping = true; // ������ ��

            if (IsWalled()) // ���̶��
            {
                jumpDirection *= -1; // �ݴ���� ����
                gameObject.transform.rotation = Quaternion.Euler(0, transform.rotation.y == 0 ? 180 : 0, 0); // �ݴ�������� ȸ��
                rb.velocity = new Vector2(wallJumpingPower.x * jumpDirection, wallJumpingPower.y); // walljupingpower��ŭ �ݴ��������
            }

            wallJumpingCounter = 0f;
            Co_StopWallJumping = StartCoroutine(co_StopWallJumping(wallJumpingDuration));
        }
    }

    /// <summary>
    /// Sprite ���� ��ȯ
    /// </summary>
    private void _Flip() // �̵��ϴ� ���� �ٶ󺸱�
    {
        if (hor < 0f || hor > 0)
        {
            if (hor > 0)
            {
                isFacingRight = 1;
                gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else if (hor < 0)
            {
                isFacingRight = -1;
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }
    #endregion
  
    #region Coroutine_Function
    IEnumerator co_StopWallJumping(float daley)
    {
        yield return new WaitForSeconds(daley);
        isWallJumping = false;
    }
    #endregion
}

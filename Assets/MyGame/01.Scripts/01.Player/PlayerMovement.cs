using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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
    public Direction playerDir;
    private float hor; // hor = Input.GetAxis("Horizontal"); �뵵
    private float isFacingRight = 1; // Flip �뵵

    Vector2 velocity;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        playerDir = Direction.Right;
    }

    private void Update()
    {
        if (!isWallJumping)
        {
            Flip();
        }

        Jump();
        wallSlide();
        WallJump();
    }

    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            Move();
        }
    }
    private void Move() // �÷��̾� ������
    {
        hor = Input.GetAxis("Horizontal");

        velocity.x = hor * PlayerStatManager.instance.speed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity; // new�� �����ϱ� ���� Vector2 velocity ���� �� �ʱ�ȭ
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            //rb.velocity = new Vector2(rb.velocity.x, PlayerStatManager.instance.JumpPoawer);
            rb.AddForce(Vector2.up * PlayerStatManager.instance.JumpPoawer, ForceMode2D.Impulse);

        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundChk.position, 0.2f, groundLayer);
        // ���� groundChk.position�� ��ġ�� 0.2f ũ���� �� �ȿ� groundLayer�� �ε����ٸ� true�� ����
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallChk.position, 0.2f, wallLayer);
        // ���� wallChk.position�� 0.2f ũ���� �� �ȿ� wallLayer�� �ε����ٸ� true�� ����
    }

    private void wallSlide() // �� �����̵�
    {
        if (IsWalled() && !IsGrounded() && hor != 0)
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

    private void WallJump()
    {
        if (iswallSliding) // �������� ������ ����
        {
            isWallJumping = false; // ������ Ȱ��ȭ
            wallJumpingCounter = wallJumpingTime; // ������ ������ �� �ִ� �ð�

            CancelInvoke(nameof(StopWallJumping)); // ������ ������ �������� �� �������� ��ҵǴ� ���� ����
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
            Invoke(nameof(StopWallJumping), wallJumpingDuration); // ���� �ð��� ������ ����
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip() // �̵��ϴ� ���� �ٶ󺸱�
    {
        if ( hor < 0f || hor > 0)
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
}

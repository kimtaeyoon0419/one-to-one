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
    private bool isFacingRight = true; // Flip �뵵

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
        hor = Input.GetAxis("Horizontal");

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
        if(hor > 0)
        {
            playerDir = Direction.Right; // ���� ���� �ʱ�ȭ ( ������ )
        }
        else if(hor < 0)
        {
            playerDir = Direction.Left; // ���� ���� �ʱ�ȭ ( ���� )
        }
        velocity.x = hor * PlayerStatManager.instance.speed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity; // new�� �����ϱ� ���� Vector2 velocity ���� �� �ʱ�ȭ
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, PlayerStatManager.instance.JumpPoawer);
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
        if (iswallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && hor < 0f || !isFacingRight && hor > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}

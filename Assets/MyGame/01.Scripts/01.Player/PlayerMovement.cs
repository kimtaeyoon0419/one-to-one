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
    private bool iswallSliding; // 현재 벽을 타고 있는지
    private bool isWallJumping; // 현재 벽점프 중인지
    private float wallSlidingSpeed = 2f; // 벽 타고 내려오는 속도
    private float wallJumpingTime = 0.2f; // 벽에서 떨어진 이후 벽점프가 가능한 시간
    private float wallJumpingCounter; // 벽점프가 가능한 시간
    private float wallJumpingDirection; // 벽점프 방향
    private float wallJumpingDuration = 0.2f; // 벽점프 지속시간
    private Vector2 wallJumpingPower = new Vector2(4f, 10f); // 벽점프 세기  

    [Header("Move")]
    public Direction playerDir;
    private float hor; // hor = Input.GetAxis("Horizontal"); 용도
    private float isFacingRight = 1; // Flip 용도

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
    private void Move() // 플레이어 움직임
    {
        hor = Input.GetAxis("Horizontal");

        velocity.x = hor * PlayerStatManager.instance.speed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity; // new를 지양하기 위해 Vector2 velocity 선언 후 초기화
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
        // 만약 groundChk.position에 위치한 0.2f 크기의 원 안에 groundLayer가 부딪힌다면 true를 리턴
    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallChk.position, 0.2f, wallLayer);
        // 만약 wallChk.position에 0.2f 크기의 원 안에 wallLayer가 부딪힌다면 true를 리턴
    }

    private void wallSlide() // 벽 슬라이딩
    {
        if (IsWalled() && !IsGrounded() && hor != 0)
        {
            iswallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            // velocity의 y값을 wallSlidingSpeed만큼 내려가기 위해 앞에 - 를 붙힘
        }
        else
        {
            iswallSliding = false;
        }
    }

    private void WallJump()
    {
        if (iswallSliding) // 벽점프가 가능한 상태
        {
            isWallJumping = false; // 벽점프 활성화
            wallJumpingCounter = wallJumpingTime; // 벽에서 점프할 수 있는 시간

            CancelInvoke(nameof(StopWallJumping)); // 벽에서 벽으로 움직였을 떄 벽점프가 취소되는 오류 방지
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            float jumpDirection = isFacingRight; // 현재 방향 저장
            isWallJumping = true; // 벽점프 중
            
            if (IsWalled()) // 벽이라면
            {
                jumpDirection *= -1; // 반대방향 설정
                gameObject.transform.rotation = Quaternion.Euler(0, transform.rotation.y == 0 ? 180 : 0, 0); // 반대방향으로 회전
                rb.velocity = new Vector2(wallJumpingPower.x * jumpDirection, wallJumpingPower.y); // walljupingpower만큼 반대방향으로
            }
            
            wallJumpingCounter = 0f;
            Invoke(nameof(StopWallJumping), wallJumpingDuration); // 일정 시간후 벽점프 종료
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip() // 이동하는 방향 바라보기
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_player : MonoBehaviour
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
    private bool isFacingRight = true; // Flip 용도
    private float yRotation = 0;

    Vector2 velocity;

    public Transform pos;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
    }

    private void Update()
    {
        hor = Input.GetAxis("Horizontal");

        if (!isWallJumping)
        {
            _Flip();
        }

        Jump();
        wallSlide();
        WallJump();
        BulletAttack();
        pos.localScale = this.transform.localScale;
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
        if (hor > 0)
        {
            playerDir = Direction.Right; // 공격 방향 초기화 ( 오른쪽 )
        }
        else if (hor < 0)
        {
            playerDir = Direction.Left; // 공격 방향 초기화 ( 왼쪽 )
        }
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
                if (yRotation == 0)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else if (yRotation == 180)
                {
                    gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void _Flip()
    {
        if (isFacingRight && hor < 0f || !isFacingRight && hor > 0)
        {
            isFacingRight = !isFacingRight;
            if (hor > 0)
            {
                yRotation = 0;
                gameObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            }
            else if (hor < 0)
            {
                yRotation = 180;
                gameObject.transform.rotation = Quaternion.Euler(0, yRotation, 0);
            }
        }
    }

    public void BulletAttack()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("x 눌림");
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // 공격 속도 초기화
            test_objcetPool.instance.Get(0, pos.transform.position, gameObject.transform.rotation); // 풀링
        }
    }
}

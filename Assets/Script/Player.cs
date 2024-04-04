using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Direction playerDir;
    public float horizontal;
    private bool isFacingRight = true;
    private bool iswallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;

    [Header("레이케스트")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask monsterLayer;

    [Header("총공격 방향 및 위치")]
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private int BulletDir = 1;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(5f, 12f);

    private bool isBoosStageGo = false; // 보스 스테이지 입장 검사
    Vector3 originalPos; // 보스입장 할 때 원래위치
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (!isWallJumping && !isBoosStageGo)
        {
            playerMove();
        }
    }

    void Update()
    {
        Fire();
        wallSlide();
        WallJump();
        playerJump();
        if (!isWallJumping)
        {
            Flip();
        }
    }

    void playerMove()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0) playerDir = Direction.Right;
        else if (horizontal < 0) playerDir = Direction.Left;
        
        rb.velocity = new Vector2(horizontal * PlayerStatManager.instance.speed, rb.velocity.y);
    }

    void playerJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded())
            {
                rb.velocity = new Vector2(rb.velocity.x, PlayerStatManager.instance.JumpPoawer);
                AudioManager.instance.PlaySFX("Jump");  
                //AudioManager.Instance.Playsfx(AudioManager.sfx.Jump);
                Debug.Log("Jump");
            }
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private bool isWalled() // 벽확인 레이케스트
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private bool IsGrounded() // 바닥확인 레이케스트
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
   

    private void WallJump() //벽점프
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

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f) //벽점프 실행
        {
            horizontal = 0;
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            if (playerDir < 0) playerDir = Direction.Right;
            else if (playerDir > 0) playerDir = Direction.Left;
            AudioManager.instance.PlaySFX("Jump");

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                BulletDir *= -1;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void wallSlide() // 벽타기
    {
        if (isWalled() && !IsGrounded() && horizontal != 0f)
        {
            iswallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            iswallSliding = false;
        }
    }

    private void Flip() // 가능방향 바라보기
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            BulletDir *= -1;
            transform.localScale = localScale;
        }
    }

    private void Fire() // 총쏘기
    {
        if (Input.GetKeyDown(KeyCode.X) && PlayerStatManager.instance.bulletshotCurTime <= 0 && PlayerStatManager.instance.CurBulletCount > 0)
        {
            PlayerStatManager.instance.CurBulletCount--;
            Debug.Log(("남은 탄수 : ") + PlayerStatManager.instance.CurBulletCount);
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime;
            GameManager.instance.pools.Get(0, bulletPosition.position, BulletDir);
        }
    }

    
}

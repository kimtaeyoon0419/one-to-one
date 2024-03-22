using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    [Header("플레이어 움직임스텟")]
    [SerializeField] float speed;
    [SerializeField] float JumpPoawer;

    public Direction playerDir;
    private float horizontal;
    private float Shothorizontal;
    private bool isFacingRight = true;

    private bool iswallSliding;
    [SerializeField] float wallSlidingSpeed = 2f;

    [Header("레이케스트")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [Header("총공격 스텟")]
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private float bulletshotCoolTime;
    [SerializeField] private float bulletshotCurTime;
    [SerializeField] private int BulletDir = 1;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(5f, 12f);

    
    Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }
    void FixedUpdate()
    {
        if (!isWallJumping)
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

        bulletshotCurTime -= Time.deltaTime;
    }


    void playerMove()
    {
        horizontal = Input.GetAxis("Horizontal"); // 1, 0, -1 

        // Player Direction
        if(horizontal > 0) playerDir = Direction.Right;
        else if(horizontal < 0) playerDir = Direction.Left;

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    void playerJump()
    {

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpPoawer);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

    }

    private bool isWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
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
            horizontal = 0;
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;
            if (playerDir < 0) playerDir = Direction.Right;
            else if (playerDir > 0) playerDir = Direction.Left;


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


    private void wallSlide()
    {
        if(isWalled() && !IsGrounded() && horizontal != 0f)
        {
            iswallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            iswallSliding = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(wallCheck.transform.position, 0.2f);
    }

    private void Flip()
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

    private void Fire()
    {
        if (Input.GetKeyDown(KeyCode.X) && bulletshotCurTime <= 0 && PlayerStatManager.instance.CurBulletCount > 0)
        {
            PlayerStatManager.instance.CurBulletCount--;
            Debug.Log(PlayerStatManager.instance.CurBulletCount);
            //bulletshotCurTime = bulletshotCoolTime;
            GameManager.instance.pools.Get(0, bulletPosition.position, BulletDir);
        }
    }
}

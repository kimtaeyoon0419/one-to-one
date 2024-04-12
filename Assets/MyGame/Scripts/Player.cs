using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    public Direction playerDir;
    public float horizontal;
    private bool isFacingRight = true;
    private bool iswallSliding;
    [SerializeField] private float wallSlidingSpeed = 2f;
    private bool isJumping = false;

    [Header("Raycast")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask monsterLayer;

    [Header("Bullet Atk")]
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private int BulletDir = 1;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    private Vector2 wallJumpingPower = new Vector2(8f, 12f);

    private bool isBoosStageGo = false; // 보스 스테이지 입장 검사
    Vector3 originalPos; // 보스입장 할 때 원래위치
    Vector2 velocity;
    Rigidbody2D rb;
    private CinemachineImpulseSource impulseSource;

    [Header("SpriteRenderer")]
    SpriteRenderer sr;
    Color hafpA = new Color(0, 0, 0);
    Color fullA = new Color(1, 1, 1);
    [SerializeField] private float delayTime;
    private WaitForSeconds waitForSeconds;

    private Vector3 jumpVec3;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        waitForSeconds = new WaitForSeconds(delayTime);
    }

private void Start()
    {
        playerDir = Direction.Right;
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

        velocity.x = horizontal * PlayerStatManager.instance.speed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;
    }

    void playerJump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (!isJumping)
            {
                rb.AddForce(Vector2.up * PlayerStatManager.instance.JumpPoawer, ForceMode2D.Impulse);
                AudioManager.instance.PlaySFX("Jump");
                isJumping = true;
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

    //private bool IsGrounded() // 바닥확인 레이케스트
    //{
    //    return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    //}


    private void WallJump() // 벽점프
    {
        if (iswallSliding) // 벽에 타고 있는가?
        {
            isWallJumping = false; // 벽에서 점프중이 아님
            wallJumpingDirection = -transform.localScale.x; // 벽의 반대 방향으로 점프
            wallJumpingCounter = wallJumpingTime; // wallJumpingTime 동안 벽점프를 할 수 있음 
            //CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f) //벽점프 실행
        {
            horizontal = 0; // 플레아어 속도를 0으로 ( 수평 속도만 0 ) 안하면 점프 도중 반대방향을 바라봄
            velocity = Vector2.zero; // 플레이어 속도를 0으로 ( 수직 수평 전부 ) 
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f; // 벽 점프가 끝난 이후 다시 점프 가능
            if (playerDir == Direction.Left) playerDir = Direction.Right;
            else if (playerDir == Direction.Right) playerDir = Direction.Left; // 벽 점프를 했을 때 반대 방향으로 초기화 ( bullet 발사 방향 용도 )
            AudioManager.instance.PlaySFX("Jump"); // 점프 사운드

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
                BulletDir *= -1;
            } // 벽점프를 했을 때 캐릭터를 반전 시킴 ( 플레이어 캐릭터 본인 방향 초기화)

            Invoke(nameof(StopWallJumping), wallJumpingDuration); // 벽 점프 상태 지속시간
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void wallSlide() // 벽타기
    {
        if (isWalled() && isJumping && horizontal != 0f)
        {
            iswallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            iswallSliding = false;
        }
    }

    private void Flip() // 가는방향 바라보기
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
    private void TakeDamage()
    {
        Debug.Log("데미지입음");
        // 플레이어 hp 다운시켜주고 오른쪽 체력바 하나 깎아주고
        // 맞았다는 표시( 사운드 or 화면 흔들림 or 광과민성 )
        // 무적코루틴실행
        // 밀려나는물리
        StartCoroutine(Co_isHit());
        CameraShakeManager.instance.CameraShake(impulseSource);
        
    }

    IEnumerator Co_isHit()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return waitForSeconds;
            sr.color = hafpA;
            yield return waitForSeconds;
            sr.color = fullA;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(PlayerStatManager.instance.ArmorDurability < 0)
        {
            PlayerStatManager.instance.Die();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}

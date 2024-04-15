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
    private bool isJumping = false;

    float isRight = 1;

    [Header("WallJump")]
    bool isWall;
    bool isGrounded;
    public bool isWallSliding;
    public float wallSlidingSpeed;
    bool isWallJumping;
    public float xWallJumpingForce;
    public float yWallJumpingForce;
    public float wallJumpTime;
    public float hor = 1;

    [Header("Raycast")]
    public Transform wallChk;
    public float wallchkDistance;
    public LayerMask w_Layer;

    public Transform groundChk;
    public float groundchkDistance;
    public LayerMask g_Layer;

    [Header("Bullet Atk")]
    [SerializeField] private Transform bulletPosition;
    [SerializeField] private int BulletDir = 1;


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
        if (isWallSliding) //벽 슬라이딩 중이라면
        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue)); // wallSlidingSpeed만큼 느려짐
        }
        playerMove();
        if (isWallJumping == true) // 벽점프 실행
        {
            rb.velocity = new Vector2(xWallJumpingForce * -isRight, yWallJumpingForce); // 벽의 반대편으로 점프
        }
    }

    void Update()
    {
        isWall = Physics2D.OverlapCircle(wallChk.position, wallchkDistance, w_Layer);
        isGrounded = Physics2D.OverlapCircle(groundChk.position, groundchkDistance, g_Layer);
        Fire();
        playerJump();
        if ((horizontal > 0 && isRight < 0) || (horizontal < 0 && isRight > 0))
        {
            Flip();
        }
        else if (horizontal == 0)
        {
            hor = 0;
        }
        if (isWall && !isGrounded && horizontal != 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
        if(Input.GetButtonDown("Jump") && isWallSliding == true)
        {
            isWallJumping = true;
            Invoke("SetWallJumpingToFalse", wallJumpTime);
        }
        
    }
    void SetWallJumpingToFalse()
    {
        isWallJumping = false;
    }

    void playerMove()
    {
        horizontal = Input.GetAxis("Horizontal");
        if (horizontal > 0)
        {
            playerDir = Direction.Right;
        }
        else if (horizontal < 0)
        {
            playerDir = Direction.Left;
        }

        velocity.x = horizontal * PlayerStatManager.instance.speed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity;
    }

    void playerJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector2.up * PlayerStatManager.instance.JumpPoawer, ForceMode2D.Impulse);
            AudioManager.instance.PlaySFX("Jump");
            isJumping = true;
            //AudioManager.Instance.Playsfx(AudioManager.sfx.Jump);
            Debug.Log("Jump");
        }
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    private void Flip() // 가는방향 바라보기
    {
        BulletDir *= -1;
        transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
        isRight = isRight * -1;
    }

    private void Fire() // 총쏘기
    {
        if (Input.GetKeyDown(KeyCode.X) && PlayerStatManager.instance.bulletshotCurTime <= 0 && PlayerStatManager.instance.CurBulletCount > 0)
        {
            PlayerStatManager.instance.CurBulletCount--;
            Debug.Log(("남은 탄수 : ") + PlayerStatManager.instance.CurBulletCount);
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime;
            GameManager.instance.pools.Get(0, bulletPosition.position);
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

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (PlayerStatManager.instance.ArmorDurability < 0)
    //    {
    //        PlayerStatManager.instance.Die();
    //    }
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isJumping = false;
        }
    }
}

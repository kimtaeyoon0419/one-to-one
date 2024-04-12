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

    private bool isBoosStageGo = false; // ���� �������� ���� �˻�
    Vector3 originalPos; // �������� �� �� ������ġ
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

    private bool isWalled() // ��Ȯ�� �����ɽ�Ʈ
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    //private bool IsGrounded() // �ٴ�Ȯ�� �����ɽ�Ʈ
    //{
    //    return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    //}


    private void WallJump() // ������
    {
        if (iswallSliding) // ���� Ÿ�� �ִ°�?
        {
            isWallJumping = false; // ������ �������� �ƴ�
            wallJumpingDirection = -transform.localScale.x; // ���� �ݴ� �������� ����
            wallJumpingCounter = wallJumpingTime; // wallJumpingTime ���� �������� �� �� ���� 
            //CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f) //������ ����
        {
            horizontal = 0; // �÷��ƾ� �ӵ��� 0���� ( ���� �ӵ��� 0 ) ���ϸ� ���� ���� �ݴ������ �ٶ�
            velocity = Vector2.zero; // �÷��̾� �ӵ��� 0���� ( ���� ���� ���� ) 
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f; // �� ������ ���� ���� �ٽ� ���� ����
            if (playerDir == Direction.Left) playerDir = Direction.Right;
            else if (playerDir == Direction.Right) playerDir = Direction.Left; // �� ������ ���� �� �ݴ� �������� �ʱ�ȭ ( bullet �߻� ���� �뵵 )
            AudioManager.instance.PlaySFX("Jump"); // ���� ����

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
                BulletDir *= -1;
            } // �������� ���� �� ĳ���͸� ���� ��Ŵ ( �÷��̾� ĳ���� ���� ���� �ʱ�ȭ)

            Invoke(nameof(StopWallJumping), wallJumpingDuration); // �� ���� ���� ���ӽð�
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void wallSlide() // ��Ÿ��
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

    private void Flip() // ���¹��� �ٶ󺸱�
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
    
    private void Fire() // �ѽ��
    {
        if (Input.GetKeyDown(KeyCode.X) && PlayerStatManager.instance.bulletshotCurTime <= 0 && PlayerStatManager.instance.CurBulletCount > 0)
        {
            PlayerStatManager.instance.CurBulletCount--;
            Debug.Log(("���� ź�� : ") + PlayerStatManager.instance.CurBulletCount);
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime;
            GameManager.instance.pools.Get(0, bulletPosition.position, BulletDir);
        }
    }
    private void TakeDamage()
    {
        Debug.Log("����������");
        // �÷��̾� hp �ٿ�����ְ� ������ ü�¹� �ϳ� ����ְ�
        // �¾Ҵٴ� ǥ��( ���� or ȭ�� ��鸲 or �����μ� )
        // �����ڷ�ƾ����
        // �з����¹���
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

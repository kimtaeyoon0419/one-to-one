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
    bool isWall;
    public float slidingSpeed;

    [Header("Raycast")]
    public Transform wallChk;
    public float wallchkDistance;
    public LayerMask w_Layer;

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
        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallchkDistance, w_Layer);
        Fire();
        //wallSlide();
        //WallJump();
        playerJump();
        if ((horizontal > 0 && isRight < 0) || (horizontal < 0 && isRight > 0))
        {
            Flip();
        }
        else if (horizontal == 0)
        {
            
        }
        if (isWall)
        {
            wallSlide();
        }
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

    private void wallSlide()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * slidingSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallchkDistance);
    }

    private void Flip() // ���¹��� �ٶ󺸱�
    {
            BulletDir *= -1;
            transform.eulerAngles = new Vector3(0, Mathf.Abs(transform.eulerAngles.y - 180), 0);
            isRight = isRight * -1;
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
        if (PlayerStatManager.instance.ArmorDurability < 0)
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

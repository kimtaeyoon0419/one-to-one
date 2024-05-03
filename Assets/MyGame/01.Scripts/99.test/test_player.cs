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
    private void Move() // �÷��̾� ������
    {
        if (hor > 0)
        {
            playerDir = Direction.Right; // ���� ���� �ʱ�ȭ ( ������ )
        }
        else if (hor < 0)
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
            Debug.Log("x ����");
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // ���� �ӵ� �ʱ�ȭ
            test_objcetPool.instance.Get(0, pos.transform.position, gameObject.transform.rotation); // Ǯ��
        }
    }
}

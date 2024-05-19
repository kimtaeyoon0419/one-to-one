using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Component")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerStats stats;
    [SerializeField] private Animator animator;

    [Header("LayCast")]
    [SerializeField] private Transform wallChk;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform groundChk;
    [SerializeField] private LayerMask groundLayer;

    [Header("WallJump")]
    private bool iswallSliding; // ���� ���� Ÿ�� �ִ���
    private bool isWallJumping; // ���� ������ ������
    private float wallSlidingSpeed = 2f; // �� Ÿ�� �������� �ӵ�
    private float wallJumpingTime = 0.2f; // ������ ������ ���� �������� ������ �ð�
    private float wallJumpingCounter; // �������� ������ �ð�
    private float wallJumpingDuration = 0.2f; // ������ ���ӽð�
    private Vector2 wallJumpingPower = new Vector2(4f, 10f); // ������ ����  

    [Header("Move")]
    private float hor; // hor = Input.GetAxis("Horizontal"); �뵵
    private float isFacingRight = 1; // Flip �뵵
    private Vector2 velocity;
    bool test_isGround;

    [Header("Coroutine")]
    private Coroutine Co_StopWallJumping;

    [Header("Animation")]
    private readonly int hashIsRun = Animator.StringToHash("IsRun");

    #region Unity_Function
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }

    private void Start()
    {
        Debug.Log("name: " + stats.charName);
        Debug.Log("speed : " + stats.speed);
        Debug.Log("Jumppower: " + stats.jumpPoawer);
    }

    private void Update()
    {
        hor = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log(_IsGround());
        }

        SetAnim();
        _Jump();
        _wallSlide();
        _WallJump();
    }
    private void FixedUpdate()
    {
        if (!isWallJumping)
        {
            _Move();
            _Flip();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundChk.position, groundChk.position + Vector3.down * 0.1f);
    }


    #endregion

    #region Private_Function
    /// <summary>
    /// ������
    /// </summary>
    private void _Move() // �÷��̾� ������
    {
        velocity.x = hor * stats.speed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity; // new�� �����ϱ� ���� Vector2 velocity ���� �� �ʱ�ȭ
    }

    /// <summary>
    /// ����
    /// </summary>
    private void _Jump()
    {
        if (Input.GetButtonDown("Jump") && _IsGround())
        {
            Debug.Log("������");
            rb.velocity = Vector2.up * stats.jumpPoawer;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }
    private void SetAnim()
    {
        if (hor != 0)
        {
            animator.SetBool(hashIsRun, true);
        }
        else if (hor == 0)
        {
            animator.SetBool(hashIsRun, false);
        }
    }

    /// <summary>
    /// ���� �پ��ִ��� üũ
    /// </summary>
    /// <returns></returns>
    private bool _IsGround()
    {
        RaycastHit2D rayhit = Physics2D.Raycast(groundChk.position, Vector2.down, 0.1f, groundLayer);
        return rayhit.collider != null;
    }

    /// <summary>
    /// ���� �پ��ִ��� üũ
    /// </summary>
    /// <returns></returns>
    private bool _IsWalled()
    {
        return Physics2D.OverlapCircle(wallChk.position, 0.2f, wallLayer);
        // ���� wallChk.position�� 0.2f ũ���� �� �ȿ� wallLayer�� �ε����ٸ� true�� ����
    }

    /// <summary>
    /// �� ���� �پ��ִ��� üũ
    /// </summary>
    /// <returns></returns>
    private bool _IsWallGround()
    {
        return Physics2D.OverlapCircle(wallChk.position, 0.2f, groundLayer);
    }

    /// <summary>
    /// �� �����̵�
    /// </summary>
    private void _wallSlide() // �� �����̵�
    {
        if (_IsWalled() || _IsWallGround())
        {
            if (hor != 0)
            {
                Debug.Log("��Ÿ����~~");
                iswallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
                // velocity�� y���� wallSlidingSpeed��ŭ �������� ���� �տ� - �� ����
            }
        }
        else
        {
            iswallSliding = false;
        }
    }

    /// <summary>
    /// ������
    /// </summary>
    private void _WallJump()
    {
        if (iswallSliding) // �������� ������ ����
        {
            isWallJumping = false; // ������ Ȱ��ȭ
            wallJumpingCounter = wallJumpingTime; // ������ ������ �� �ִ� �ð�

            if (Co_StopWallJumping != null) StopCoroutine(Co_StopWallJumping);
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f && !_IsGround())
        {
            float jumpDirection = isFacingRight; // ���� ���� ����
            isWallJumping = true; // ������ ��

            if (_IsWalled() || _IsWallGround()) // ���̶��
            {
                jumpDirection *= -1; // �ݴ���� ����
                gameObject.transform.rotation = Quaternion.Euler(0, transform.rotation.y == 0 ? 180 : 0, 0); // �ݴ�������� ȸ��
                rb.velocity = new Vector2(wallJumpingPower.x * jumpDirection, wallJumpingPower.y); // walljupingpower��ŭ �ݴ��������
            }
            wallJumpingCounter = 0f;
            Co_StopWallJumping = StartCoroutine(co_StopWallJumping(wallJumpingDuration));
        }
    }

    /// <summary>
    /// Sprite ���� ��ȯ
    /// </summary>
    private void _Flip() // �̵��ϴ� ���� �ٶ󺸱�
    {
        if (hor < 0f || hor > 0)
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
    #endregion

    #region Coroutine_Function
    IEnumerator co_StopWallJumping(float daley)
    {
        yield return new WaitForSeconds(daley);
        isWallJumping = false;
    }
    #endregion
}

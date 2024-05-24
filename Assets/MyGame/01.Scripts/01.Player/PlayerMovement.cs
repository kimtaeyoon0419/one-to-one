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
    private bool iswallSliding; // 현재 벽을 타고 있는지
    private bool isWallJumping; // 현재 벽점프 중인지
    private float wallSlidingSpeed = 2f; // 벽 타고 내려오는 속도
    private float wallJumpingTime = 0.2f; // 벽에서 떨어진 이후 벽점프가 가능한 시간
    private float wallJumpingCounter; // 벽점프가 가능한 시간
    private float wallJumpingDuration = 0.2f; // 벽점프 지속시간
    private Vector2 wallJumpingPower = new Vector2(4f, 10f); // 벽점프 세기  

    [Header("Move")]
    private float hor; // hor = Input.GetAxis("Horizontal"); 용도
    private float isFacingRight = 1; // Flip 용도
    private Vector2 velocity;

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
        Gizmos.DrawLine(groundChk.position, groundChk.position + Vector3.down * 0.15f);
    }


    #endregion

    #region Private_Function
    /// <summary>
    /// 움직임
    /// </summary>
    private void _Move() // 플레이어 움직임
    {
        velocity.x = hor * stats.speed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity; // new를 지양하기 위해 Vector2 velocity 선언 후 초기화
    }

    /// <summary>
    /// 점프
    /// </summary>
    private void _Jump()
    {
        if (Input.GetButtonDown("Jump") && _IsGround())
        {
            Debug.Log("점프함");
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
    /// 땅에 붙어있는지 체크
    /// </summary>
    /// <returns></returns>
    private bool _IsGround()
    {
        RaycastHit2D rayhit = Physics2D.Raycast(groundChk.position, Vector2.down, 0.15f, groundLayer);
        return rayhit.collider != null;
    }

    /// <summary>
    /// 벽에 붙어있는지 체크
    /// </summary>
    /// <returns></returns>
    private bool _IsWalled()
    {
        return Physics2D.OverlapCircle(wallChk.position, 0.2f, wallLayer);
        // 만약 wallChk.position에 0.2f 크기의 원 안에 wallLayer가 부딪힌다면 true를 리턴
    }

    /// <summary>
    /// 땅 옆에 붙어있는지 체크
    /// </summary>
    /// <returns></returns>
    private bool _IsWallGround()
    {
        return Physics2D.OverlapCircle(wallChk.position, 0.2f, groundLayer);
    }

    /// <summary>
    /// 벽 슬라이딩
    /// </summary>
    private void _wallSlide() // 벽 슬라이딩
    {
        if (_IsWalled() || _IsWallGround())
        {
            if (hor != 0)
            {
                Debug.Log("벽타는중~~");
                iswallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
                // velocity의 y값을 wallSlidingSpeed만큼 내려가기 위해 앞에 - 를 붙힘
            }
        }
        else
        {
            iswallSliding = false;
        }
    }

    /// <summary>
    /// 벽점프
    /// </summary>
    private void _WallJump()
    {
        if (iswallSliding) // 벽점프가 가능한 상태
        {
            isWallJumping = false; // 벽점프 활성화
            wallJumpingCounter = wallJumpingTime; // 벽에서 점프할 수 있는 시간

            if (Co_StopWallJumping != null) StopCoroutine(Co_StopWallJumping);
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f && !_IsGround())
        {
            float jumpDirection = isFacingRight; // 현재 방향 저장
            isWallJumping = true; // 벽점프 중

            if (_IsWalled() || _IsWallGround()) // 벽이라면
            {
                jumpDirection *= -1; // 반대방향 설정
                gameObject.transform.rotation = Quaternion.Euler(0, transform.rotation.y == 0 ? 180 : 0, 0); // 반대방향으로 회전
                rb.velocity = new Vector2(wallJumpingPower.x * jumpDirection, wallJumpingPower.y); // walljupingpower만큼 반대방향으로
            }
            wallJumpingCounter = 0f;
            Co_StopWallJumping = StartCoroutine(co_StopWallJumping(wallJumpingDuration));
        }
    }

    /// <summary>
    /// Sprite 방향 전환
    /// </summary>
    private void _Flip() // 이동하는 방향 바라보기
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

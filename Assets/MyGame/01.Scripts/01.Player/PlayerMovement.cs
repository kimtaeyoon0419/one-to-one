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
    [SerializeField] private LayerMask monsterLayer;
    private Vector2 raybox = new Vector2(0.7f, 0);

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
    private bool isJumping;
    private bool isPortal;

    [Header("Coroutine")]
    private Coroutine Co_StopWallJumping;

    [Header("Animation")]
    private readonly int hashIsRun = Animator.StringToHash("IsRun");
    private readonly int hashJump = Animator.StringToHash("Jump");
    private readonly int hashJumpEnd = Animator.StringToHash("JumpEnd");
    private readonly int hashIsWall = Animator.StringToHash("IsWall");

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

        if(isPortal)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameManager.instance.nextSceneCheck = true;
            }
        }

        SetAnim();
        _Jump();
        _wallSlide();
        _WallJump();
        //stepAttack();
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

        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(groundChk.position, raybox);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Portal"))
        {
            isPortal = true;
        }
        else
        {
            isPortal = false;
        }
        if (collision.CompareTag("Item"))
        {
            stats.armorDurability += collision.GetComponent<clearItem>().armor;
            stats.speed += collision.GetComponent<clearItem>().speed;
            PlayerStats.attackPower += collision.GetComponent<clearItem>().attack;

            Destroy(collision.gameObject);
        }
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
            isJumping = true;
            animator.SetTrigger(hashJump);
            StartCoroutine(JumpEndCheck());
            rb.velocity = Vector2.up * stats.jumpPoawer;
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
    }

    /// <summary>
    /// 점프가 끝났는지 확인
    /// </summary>
    /// <returns></returns>
    IEnumerator JumpEndCheck()
    {
        yield return new WaitForSeconds(0.2f);
        while (isJumping)
        {
            if (isJumping && _IsGround())
            {
                isJumping = false;
                animator.SetTrigger(hashJumpEnd);
                break;
            }
            if (_IsWalled() || _IsWallGround())
            {
                animator.SetTrigger(hashJumpEnd);
                break;
            }
            yield return null;
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

    //private void stepAttack()
    //{
    //    Collider2D collider2D = Physics2D.OverlapBox(groundChk.position, raybox, 0, monsterLayer);
    //    collider2D.GetComponent<Monster>().TakeDmg(PlayerStats.attackPower);
    //}


    /// <summary>
    /// 땅에 붙어있는지 체크
    /// </summary>
    /// <returns></returns>
    private bool _IsGround()
    {
        //RaycastHit2D rayhit = Physics2D.Raycast(groundChk.position, Vector2.down, 0.15f, groundLayer);
        //return rayhit.collider != null;

        return Physics2D.OverlapBox(groundChk.position, raybox, 1, groundLayer);
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
                animator.SetBool(hashIsWall, true);
                isJumping = false;
                iswallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
                // velocity의 y값을 wallSlidingSpeed만큼 내려가기 위해 앞에 - 를 붙힘
            }
            if (_IsGround())
            {
                animator.SetBool(hashIsWall, false);
                isJumping = false;
                iswallSliding = false;
            }
        }
        else
        {
            animator.SetBool(hashIsWall, false);
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
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            float jumpDirection = isFacingRight; // 현재 방향 저장
            isWallJumping = true; // 벽점프 중

            if (_IsWalled() || _IsWallGround() && !_IsGround()) // 벽이라면
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

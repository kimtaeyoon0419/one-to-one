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
    /// ������ �������� Ȯ��
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
    /// ���� �پ��ִ��� üũ
    /// </summary>
    /// <returns></returns>
    private bool _IsGround()
    {
        //RaycastHit2D rayhit = Physics2D.Raycast(groundChk.position, Vector2.down, 0.15f, groundLayer);
        //return rayhit.collider != null;

        return Physics2D.OverlapBox(groundChk.position, raybox, 1, groundLayer);
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
                animator.SetBool(hashIsWall, true);
                isJumping = false;
                iswallSliding = true;
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
                // velocity�� y���� wallSlidingSpeed��ŭ �������� ���� �տ� - �� ����
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
        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            float jumpDirection = isFacingRight; // ���� ���� ����
            isWallJumping = true; // ������ ��

            if (_IsWalled() || _IsWallGround() && !_IsGround()) // ���̶��
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

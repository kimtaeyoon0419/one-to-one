using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public MonsterStat stat; // 몬스터의 스텟
    public MonsterUnitCode unitCode; // 유닛코드 지정

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;
    Color hafpA = new Color(1, 1, 1, 0.5f); // 피격 색전환 1번 ( 반투명 )
    Color fullA = new Color(1, 1, 1, 1); // 피격 색전환 2번 ( 원본색 )

    [SerializeField] private float delayTime; // WaitForSeconds 값
    [SerializeField] private GameObject AttackCollider; // 공격 범위
    [SerializeField] private bool isGround; // 바닥 검사
    private WaitForSeconds waitForSeconds; // 웨잇포세컨드

    public int nextMove; // 다음으로 움직일 방향

    private Vector2 frontVec;
    private Vector2 JumpPower = new Vector2(2f, 4f);

    private int rayLookDir; // 몬스터의 방향과 맞는 레이 방향

    private void Awake()
    {
        stat = new MonsterStat();
        stat = stat.SetUnitStatus(unitCode);
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        waitForSeconds = new WaitForSeconds(delayTime);
    }

    void Start()
    {
        StartCoroutine(Co_Think());
    }

    private void FixedUpdate()
    {
        print("호출");
        Vector2 DirVec = new Vector2(nextMove * stat.moveSpeed, rb.velocity.y); // 방향 설정
        rb.velocity = DirVec; // 방향으로 이동

        if (isGround == true)
        {
            frontVec = new Vector2(rb.position.x + nextMove, rb.position.y); // frontVec = 몬스터의 현재위치 + nextMove
        }
        Debug.DrawRay(frontVec, Vector2.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 3f, LayerMask.GetMask("Ground")); // frontVec만큼의 거리에 바닥이 있는지 검사
        if (rayHit.collider == null) // forntVec만큼 떨어진 거리에 땅이 없다면 회전
        {
            Turn();
            StartCoroutine(Co_StartThinkCoroutineDelay(1f));
        }
    }
    void Update()
    {
        AttackRay(); // 공격범위에 플레이어가 있는지 검사 & 공격
        if (stat.curAtkSpeed > 0)
        {
            stat.curAtkSpeed -= Time.deltaTime;
        }
        if (nextMove == 1) // 스프라이트 방향
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (nextMove == -1) // 스프라이트 방향
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    IEnumerator Co_Think() // 어느방향으로 움직일지 생각해주는 코루틴(재귀)
    {
        nextMove = Random.Range(-1, 2);

        float nextThinkTime = Random.Range(2f, 5f);
        if(this.gameObject.activeSelf)
        {
            yield return new WaitForSeconds(nextThinkTime);
            yield return StartCoroutine(Co_Think());
        }
        else
        {
            yield break;
        }
    }
    IEnumerator Co_StartThinkCoroutineDelay(float delay) // Think() 코루틴을 일정 시간 뒤에 실행해줌
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(Co_Think());
    }

    void Turn() // 방향전환
    {
        nextMove *= -1;
        StopCoroutine(Co_Think());
    }

    public void TakeDmg(int damge) // 몬스터 피격
    {
        stat.curHp -= damge;
        if (stat.curHp <= 0)
        {
            gameObject.active = false;
        }
        else StartCoroutine(Co_isHit());
    }

    IEnumerator Co_isHit() // 맞았을 때 색전환
    {
        for (int i = 0; i < 3; i++)
        {
            yield return waitForSeconds;
            sr.color = hafpA;
            yield return waitForSeconds;
            sr.color = fullA;
        }
    }
    private void AttackRay() //레이케스트에 플레이어가 들어오면 공격!!!
    {
        if (nextMove != 0)
        {
            rayLookDir = nextMove;
        }
        Debug.DrawRay(rb.position, Vector2.right * stat.atkRange * rayLookDir, Color.yellow); // 레이확인
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right * rayLookDir, stat.atkRange, LayerMask.GetMask("Player"));

        if (hit.collider != null && stat.curAtkSpeed <= 0 && isGround)
        {
            stat.curAtkSpeed = stat.atkSpeed;
            isGround = false;
            StopCoroutine(Co_Think()); // 코루틴을 꺼서 방향 전환을 막음
            Attack(); // 공격 후
            animator.SetTrigger("Attack"); // 공격 애니메이션 
            if (isGround == true)
            {
                StartCoroutine(Co_StartThinkCoroutineDelay(2f)); // 다시 방향전환을 정함
            }
        }
    }

    private void Attack() // 몬스터 공격
    {
        Debug.Log("공격!!!");
        CancelInvoke(); // 방향이 바뀌지 않게 캔슬
        //rb.AddForce(Vector2.up * 4, ForceMode2D.Impulse); // 점프 공격
        rb.velocity = JumpPower; // 점프를!
        rb.velocity = new Vector2(rb.velocity.x * stat.moveSpeed * rayLookDir, rb.velocity.y); // 레이의 방향으로 한다!
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // 현재 땅에 붙어있는지 확인
        {
            isGround = true;
        }
    }
}


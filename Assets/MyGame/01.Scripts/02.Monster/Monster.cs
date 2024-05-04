using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [Header("Stat")]
    protected MonsterStat stat; // 몬스터의 스텟
    protected MonsterUnitCode unitCode; // 유닛코드 지정

    [Header("Component")]
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer sr;
    protected Transform target_Player;
    Color hafpA = new Color(1, 1, 1, 0.5f); // 피격 색전환 1번 ( 반투명 )
    Color fullA = new Color(1, 1, 1, 1); // 피격 색전환 2번 ( 원본색 )

    [Header("WaitForSecond")]
    WaitForSeconds waitForSeconds; // 웨잇포세컨드
    protected float delayTime; // WaitForSeconds 값

    [Header("Move")]
    protected int nextMove; // 다음으로 움직일 방향
    private Vector2 frontVec; // 앞에 땅이 있는지 확인하는 거리
    int rayLookDir; // 몬스터의 방향과 맞는 레이 방향
    protected bool isAttack = false; // 공격중인지
    private Vector3 velocity;
    protected float player_Distance;


    [Header("DropItem")]
    private DropItem itemdrop;

    #region Unity_Function
    protected virtual void Awake()
    {
        stat = new MonsterStat();
        stat = stat.SetUnitStatus(unitCode);
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        waitForSeconds = new WaitForSeconds(delayTime);
        itemdrop = GetComponent<DropItem>();
        target_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Start()
    {
        StartCoroutine(Co_Think());
    }

    protected virtual void FixedUpdate()
    {
        velocity.x = nextMove * stat.moveSpeed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity; // new를 지양하기 위해 Vector2 velocity 선언 후 초기화

        if (!isAttack)
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
    protected virtual void Update()
    {
        player_Distance = Vector3.Distance(transform.position, target_Player.transform.position);
        //AttackRay(); // 공격범위에 플레이어가 있는지 검사 & 공격
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
    #endregion

    #region Private_Function
    private void Turn() // 방향전환
    {
        nextMove *= -1;
        StopCoroutine(Co_Think());
    }
    /// <summary>
    /// 플레이어가 있는지 검사하는 레이케이스
    /// </summary>
    protected void AttackRay() //레이케스트에 플레이어가 들어오면 공격!!!
    {
        if (nextMove != 0)
        {
            rayLookDir = nextMove;
        }
        Debug.DrawRay(rb.position, Vector2.right * stat.atkRange * rayLookDir, Color.yellow); // 레이확인
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right * rayLookDir, stat.atkRange, LayerMask.GetMask("Player"));

        if (hit.collider != null && stat.curAtkSpeed <= 0)
        {
            stat.curAtkSpeed = stat.atkSpeed;
            StopCoroutine(Co_Think()); // 코루틴을 꺼서 방향 전환을 막음
            isAttack = true;
            Attack(); // 공격 후
            if (isAttack == false)
            {
                StartCoroutine(Co_StartThinkCoroutineDelay(2f)); // 다시 방향전환을 정함
            }
        }
    }
    #endregion

    #region Public_Function
    /// <summary>
    /// 피격 데미지
    /// </summary>
    /// <param name="damge"></param>
    public void TakeDmg(int damge) // 몬스터 피격
    {
        stat.curHp -= damge;
        if (stat.curHp <= 0)
        {
            itemdrop.DropCoin();
            gameObject.active = false;
        }
        else StartCoroutine(Co_isHit());
    }
    #endregion

    #region Protected_Function
    /// <summary>
    /// 공격
    /// </summary>
    protected abstract void Attack();// 몬스터 공격
    #endregion

    #region Coroutine_Function
    IEnumerator Co_Think() // 어느방향으로 움직일지 생각해주는 코루틴(재귀)
    {
        nextMove = Random.Range(-1, 2);

        float nextThinkTime = Random.Range(2f, 5f);
        if (this.gameObject.activeSelf)
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
    /// <summary>
    /// 피격 했을 때 반짝거림
    /// </summary>
    /// <returns></returns>
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
    #endregion
}

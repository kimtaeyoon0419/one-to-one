using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public MonsterStat stat; // 몬스터의 스텟
    public MonsterUnitCode unitCode;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;
    Color hafpA = new Color(1, 1, 1, 0.5f);
    Color fullA = new Color(1, 1, 1, 1);

    [SerializeField] private float delayTime; // WaitForSeconds 값
    [SerializeField] private GameObject AttackCollider; // 공격 범위
    private WaitForSeconds waitForSeconds;

    LayerMask Layer;
    Vector2 velocity;

    public int nextMove;
    public bool isFacingRight;
    public float speed;

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
        Think();

        Invoke("Think", 5);
    }

    private void FixedUpdate()
    {
        print("호출");
        Vector2 DirVec = new Vector2(nextMove * speed, rb.velocity.y); // 방향 설정
        rb.velocity = DirVec; // 방향으로 이동

        Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y); // frontVec = 몬스터의 현재위치 + nextMove
        Debug.DrawRay(frontVec, Vector2.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 1f, LayerMask.GetMask("Ground")); // frontVec만큼의 거리에 바닥이 있는지 검사
        if (rayHit.collider == null) // forntVec만큼 떨어진 거리에 땅이 없다면 회전
        {
            Turn();
            Invoke("Think", 1f);
        }
    }
    void Update()
    {
        AttackRay();
                
        if (stat.curAtkSpeed > 0)
        {
            stat.curAtkSpeed -= Time.deltaTime;
        }
    }

    //재귀 함수
    void Think()
    {
        //Set next Acrive
        nextMove = Random.Range(-1, 2); // -1, 0, 1
        
        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }

    void Turn()
    {
        nextMove *= -1;
        CancelInvoke();
    }

    public void TakeDmg(int damge) // 몬스터 피격
    {
        stat.curHp -= damge;
        Debug.Log("아파 내 체력 : " + stat.curHp);
        if (stat.curHp <= 0)
        {
            gameObject.active = false;
        }
        else StartCoroutine(Co_isHit());
    }

    IEnumerator Co_isHit() // 맞았을 때 색
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
        Debug.DrawRay(rb.position, Vector2.right * stat.atkRange, Color.yellow);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right, stat.atkRange, LayerMask.GetMask("Player"));
        if (hit.collider != null && stat.curAtkSpeed <= 0)
        {
            stat.curAtkSpeed = stat.atkSpeed;
            Attack();
            Invoke("Think", 2f);
            animator.SetTrigger("Attack");
        }
    }

    private void Attack() // 몬스터 공격
    {
        Debug.Log("공격!!!");
        CancelInvoke(); // 방향이 바뀌지 않게 캔슬
        rb.AddForce(Vector2.up * stat.atkRange, ForceMode2D.Impulse); // 점프 공격
    }
}


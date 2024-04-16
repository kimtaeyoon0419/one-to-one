using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public MonsterStat stat; // ������ ����
    public MonsterUnitCode unitCode;

    MonsterMove monsterMove;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;
    Color hafpA = new Color(1, 1, 1, 0.5f);
    Color fullA = new Color(1, 1, 1, 1);

    [SerializeField] private float delayTime; // WaitForSeconds ��
    [SerializeField] private GameObject AttackCollider; // ���� ����
    private WaitForSeconds waitForSeconds;

    LayerMask Layer;
    Vector2 velocity;

    public int nextMove;
    public bool isFacingRight;

    private void Awake()
    {
        stat = new MonsterStat();
        stat = stat.SetUnitStatus(unitCode);
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        waitForSeconds = new WaitForSeconds(delayTime);
        monsterMove = GetComponent<MonsterMove>();
    }

    void Start()
    {
        Think();

        Invoke("Think", 5);
    }

    private void FixedUpdate()
    {
        monsterMove.Move(nextMove, stat.moveSpeed);

        Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y); // ������ ����
        Debug.DrawRay(frontVec, Vector2.down, new Color(0,1,0)); 
        RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector2.down, 1f, LayerMask.GetMask("Ground")); // frontVec��ŭ�� �Ÿ��� �ٴ��� �ִ��� �˻�
        if(rayHit.collider == null)
        {
            Turn();
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

    //��� �Լ�
    void Think()
    {
        //Set next Acrive
        nextMove = Random.Range(-1, 2);
        

        //Recursive
        float nextThinkTime = Random.Range(2f, 5f);
        Invoke("Think", nextThinkTime);

    }
    void Turn()
    {
        if (isFacingRight && nextMove < 0f || !isFacingRight && nextMove > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    public void TakeDmg(int damge) // ���� �ǰ�
    {
        stat.curHp -= damge;
        Debug.Log("���� �� ü�� : " + stat.curHp);
        if (stat.curHp <= 0)
        {
            gameObject.active = false;
        }
        else StartCoroutine(Co_isHit());
    }

    IEnumerator Co_isHit() // �¾��� �� ��
    {
        for (int i = 0; i < 3; i++)
        {
            yield return waitForSeconds;
            sr.color = hafpA;
            yield return waitForSeconds;
            sr.color = fullA;
        }
    }
    private void AttackRay() //�����ɽ�Ʈ�� �÷��̾ ������ ����!!!
    {
        Debug.DrawRay(rb.position, Vector2.right * stat.atkRange, Color.yellow);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right, stat.atkRange, LayerMask.GetMask("Player"));
        if (hit.collider != null && stat.curAtkSpeed <= 0)
        {
            stat.curAtkSpeed = stat.atkSpeed;
            animator.SetTrigger("Attack");
        }
    }

    private void Attak() // �ִϸ����Ϳ��� ȣ��
    {
        StartCoroutine(Co_attack());
        Debug.Log("Dd)");
    }

    private IEnumerator Co_attack()
    {
        AttackCollider.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        AttackCollider.SetActive(false);
    }
}


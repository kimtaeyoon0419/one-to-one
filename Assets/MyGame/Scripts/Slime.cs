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
        print("ȣ��");
        Vector2 DirVec = new Vector2(nextMove * speed, rb.velocity.y); // ���� ����
        rb.velocity = DirVec; // �������� �̵�

        Vector2 frontVec = new Vector2(rb.position.x + nextMove, rb.position.y); // frontVec = ������ ������ġ + nextMove
        Debug.DrawRay(frontVec, Vector2.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 1f, LayerMask.GetMask("Ground")); // frontVec��ŭ�� �Ÿ��� �ٴ��� �ִ��� �˻�
        if (rayHit.collider == null) // forntVec��ŭ ������ �Ÿ��� ���� ���ٸ� ȸ��
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

    //��� �Լ�
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
            Attack();
            Invoke("Think", 2f);
            animator.SetTrigger("Attack");
        }
    }

    private void Attack() // ���� ����
    {
        Debug.Log("����!!!");
        CancelInvoke(); // ������ �ٲ��� �ʰ� ĵ��
        rb.AddForce(Vector2.up * stat.atkRange, ForceMode2D.Impulse); // ���� ����
    }
}


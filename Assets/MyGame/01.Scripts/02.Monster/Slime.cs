using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : MonoBehaviour
{
    public MonsterStat stat; // ������ ����
    public MonsterUnitCode unitCode; // �����ڵ� ����

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;
    Color hafpA = new Color(1, 1, 1, 0.5f); // �ǰ� ����ȯ 1�� ( ������ )
    Color fullA = new Color(1, 1, 1, 1); // �ǰ� ����ȯ 2�� ( ������ )

    [SerializeField] private float delayTime; // WaitForSeconds ��
    [SerializeField] private GameObject AttackCollider; // ���� ����
    [SerializeField] private bool isGround; // �ٴ� �˻�
    private WaitForSeconds waitForSeconds; // ������������

    public int nextMove; // �������� ������ ����

    private Vector2 frontVec;
    private Vector2 JumpPower = new Vector2(2f, 4f);

    private int rayLookDir; // ������ ����� �´� ���� ����

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
        print("ȣ��");
        Vector2 DirVec = new Vector2(nextMove * stat.moveSpeed, rb.velocity.y); // ���� ����
        rb.velocity = DirVec; // �������� �̵�

        if (isGround == true)
        {
            frontVec = new Vector2(rb.position.x + nextMove, rb.position.y); // frontVec = ������ ������ġ + nextMove
        }
        Debug.DrawRay(frontVec, Vector2.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 3f, LayerMask.GetMask("Ground")); // frontVec��ŭ�� �Ÿ��� �ٴ��� �ִ��� �˻�
        if (rayHit.collider == null) // forntVec��ŭ ������ �Ÿ��� ���� ���ٸ� ȸ��
        {
            Turn();
            StartCoroutine(Co_StartThinkCoroutineDelay(1f));
        }
    }
    void Update()
    {
        AttackRay(); // ���ݹ����� �÷��̾ �ִ��� �˻� & ����
        if (stat.curAtkSpeed > 0)
        {
            stat.curAtkSpeed -= Time.deltaTime;
        }
        if (nextMove == 1) // ��������Ʈ ����
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (nextMove == -1) // ��������Ʈ ����
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    IEnumerator Co_Think() // ����������� �������� �������ִ� �ڷ�ƾ(���)
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
    IEnumerator Co_StartThinkCoroutineDelay(float delay) // Think() �ڷ�ƾ�� ���� �ð� �ڿ� ��������
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(Co_Think());
    }

    void Turn() // ������ȯ
    {
        nextMove *= -1;
        StopCoroutine(Co_Think());
    }

    public void TakeDmg(int damge) // ���� �ǰ�
    {
        stat.curHp -= damge;
        if (stat.curHp <= 0)
        {
            gameObject.active = false;
        }
        else StartCoroutine(Co_isHit());
    }

    IEnumerator Co_isHit() // �¾��� �� ����ȯ
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
        if (nextMove != 0)
        {
            rayLookDir = nextMove;
        }
        Debug.DrawRay(rb.position, Vector2.right * stat.atkRange * rayLookDir, Color.yellow); // ����Ȯ��
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right * rayLookDir, stat.atkRange, LayerMask.GetMask("Player"));

        if (hit.collider != null && stat.curAtkSpeed <= 0 && isGround)
        {
            stat.curAtkSpeed = stat.atkSpeed;
            isGround = false;
            StopCoroutine(Co_Think()); // �ڷ�ƾ�� ���� ���� ��ȯ�� ����
            Attack(); // ���� ��
            animator.SetTrigger("Attack"); // ���� �ִϸ��̼� 
            if (isGround == true)
            {
                StartCoroutine(Co_StartThinkCoroutineDelay(2f)); // �ٽ� ������ȯ�� ����
            }
        }
    }

    private void Attack() // ���� ����
    {
        Debug.Log("����!!!");
        CancelInvoke(); // ������ �ٲ��� �ʰ� ĵ��
        //rb.AddForce(Vector2.up * 4, ForceMode2D.Impulse); // ���� ����
        rb.velocity = JumpPower; // ������!
        rb.velocity = new Vector2(rb.velocity.x * stat.moveSpeed * rayLookDir, rb.velocity.y); // ������ �������� �Ѵ�!
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) // ���� ���� �پ��ִ��� Ȯ��
        {
            isGround = true;
        }
    }
}


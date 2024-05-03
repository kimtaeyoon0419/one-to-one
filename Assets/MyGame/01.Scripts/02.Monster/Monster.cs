using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [Header("Stat")]
    public MonsterStat stat; // ������ ����
    public MonsterUnitCode unitCode; // �����ڵ� ����

    [Header("Component")]
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer sr;
    Color hafpA = new Color(1, 1, 1, 0.5f); // �ǰ� ����ȯ 1�� ( ������ )
    Color fullA = new Color(1, 1, 1, 1); // �ǰ� ����ȯ 2�� ( ������ )

    [Header("WaitForSecond")]
    WaitForSeconds waitForSeconds; // ������������
    public float delayTime; // WaitForSeconds ��

    [Header("Move")]
    public bool isGround; // �ٴ� �˻�
    public int nextMove; // �������� ������ ����
    private Vector2 frontVec;
    int rayLookDir; // ������ ����� �´� ���� ����


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
    }

    void Start()
    {
        StartCoroutine(Co_Think());
    }

    protected virtual void FixedUpdate()
    {
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
    protected virtual void Update()
    {
        //AttackRay(); // ���ݹ����� �÷��̾ �ִ��� �˻� & ����
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
    #endregion

    #region Private_Function
    private void Turn() // ������ȯ
    {
        nextMove *= -1;
        StopCoroutine(Co_Think());
    }
    /// <summary>
    /// �÷��̾ �ִ��� �˻��ϴ� �������̽�
    /// </summary>
    protected void AttackRay() //�����ɽ�Ʈ�� �÷��̾ ������ ����!!!
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

    #endregion

    #region Public_Function
    /// <summary>
    /// �ǰ� ������
    /// </summary>
    /// <param name="damge"></param>
    public void TakeDmg(int damge) // ���� �ǰ�
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
    /// ����
    /// </summary>
    protected abstract void Attack();// ���� ����
    #endregion

    #region Coroutine_Function
    IEnumerator Co_Think() // ����������� �������� �������ִ� �ڷ�ƾ(���)
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
    IEnumerator Co_StartThinkCoroutineDelay(float delay) // Think() �ڷ�ƾ�� ���� �ð� �ڿ� ��������
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(Co_Think());
    }
    /// <summary>
    /// �ǰ� ���� �� ��¦�Ÿ�
    /// </summary>
    /// <returns></returns>
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
    #endregion
}

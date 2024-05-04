using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    [Header("Stat")]
    protected MonsterStat stat; // ������ ����
    protected MonsterUnitCode unitCode; // �����ڵ� ����

    [Header("Component")]
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer sr;
    protected Transform target_Player;
    Color hafpA = new Color(1, 1, 1, 0.5f); // �ǰ� ����ȯ 1�� ( ������ )
    Color fullA = new Color(1, 1, 1, 1); // �ǰ� ����ȯ 2�� ( ������ )

    [Header("WaitForSecond")]
    WaitForSeconds waitForSeconds; // ������������
    protected float delayTime; // WaitForSeconds ��

    [Header("Move")]
    protected int nextMove; // �������� ������ ����
    private Vector2 frontVec; // �տ� ���� �ִ��� Ȯ���ϴ� �Ÿ�
    int rayLookDir; // ������ ����� �´� ���� ����
    protected bool isAttack = false; // ����������
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

        rb.velocity = velocity; // new�� �����ϱ� ���� Vector2 velocity ���� �� �ʱ�ȭ

        if (!isAttack)
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
        player_Distance = Vector3.Distance(transform.position, target_Player.transform.position);
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

        if (hit.collider != null && stat.curAtkSpeed <= 0)
        {
            stat.curAtkSpeed = stat.atkSpeed;
            StopCoroutine(Co_Think()); // �ڷ�ƾ�� ���� ���� ��ȯ�� ����
            isAttack = true;
            Attack(); // ���� ��
            if (isAttack == false)
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

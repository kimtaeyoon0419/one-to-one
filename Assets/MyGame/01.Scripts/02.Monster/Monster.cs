using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DropItem))]
public abstract class Monster : MonoBehaviour
{
    [Header("Stat")]
    protected MonsterStat stat; // ������ ����
    [SerializeField] protected MonsterUnitCode unitCode; // �����ڵ� ����

    [Header("Component")]
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer sr;
    protected Transform target_Player;
    private DropItem itemdrop;
    Color hafpA = new Color(1, 1, 1, 0.5f); // �ǰ� ����ȯ 1�� ( ������ )
    Color fullA = new Color(1, 1, 1, 1); // �ǰ� ����ȯ 2�� ( ������ )

    [Header("WaitForSecond")]
    WaitForSeconds waitForSeconds; // ������������
    [SerializeField] protected float delayTime; // WaitForSeconds ��

    [Header("Move")]
    [SerializeField] protected int nextMove; // �������� ������ ����
    private Vector2 frontVec; // �տ� ���� �ִ��� Ȯ���ϴ� �Ÿ�
    [SerializeField] protected bool isAttack = false; // ����������
    private Vector3 velocity;
    protected float player_Distance;

    [Header("State")]
    private bool isDie = false;

    [Header("Ray")]
    [SerializeField] private Transform findPlayerPos;
    [SerializeField] private LayerMask Player;

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

    protected virtual void OnEnable()
    {
        StartCoroutine(Co_Think());
    }

    protected virtual void FixedUpdate()
    {
        velocity.x = nextMove * stat.moveSpeed;
        velocity.y = rb.velocity.y;

        rb.velocity = velocity; // new�� �����ϱ� ���� Vector2 velocity ���� �� �ʱ�ȭ

        if (_IsFollow())
        {
            FollowPlayer();
        }
        if (!isAttack)
        {
            frontVec = new Vector2(rb.position.x + nextMove, rb.position.y); // frontVec = ������ ������ġ + nextMove
            Debug.DrawRay(frontVec, Vector2.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 2.5f, LayerMask.GetMask("Ground")); // frontVec��ŭ�� �Ÿ��� �ٴ��� �ִ��� �˻�
            if (rayHit.collider == null) // forntVec��ŭ ������ �Ÿ��� ���� ���ٸ� ȸ��
            {
                Debug.Log("���� ����!");
                Turn();
                StartCoroutine(Co_StartThinkCoroutineDelay(1f));
            }
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
        if(stat.curHp <= 0)
        {
            isDie = true;
            StopAllCoroutines();
            itemdrop.DropCoin();
            itemdrop.DropGun();
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region Private_Function
    private void Turn() // ������ȯ
    {
        nextMove *= -1;
        StopCoroutine(Co_Think());
    }
    private void FollowPlayer()
    {
        Debug.Log("���󰡴���~~");
        if (target_Player.transform.position.x - transform.position.x < 0.2f)
        {
            nextMove = -1;
        }
        else if (target_Player.transform.position.x - transform.position.x > 0.2f)
        {
            nextMove = 1;
        }
    }
    private bool _IsFollow()
    {
        return Physics2D.OverlapCircle(findPlayerPos.transform.position, 2f, Player);
    }
    #endregion

    #region Public_Function
    /// <summary>
    /// �ǰ� ������
    /// </summary>
    /// <param name="damge">���� ���ط�</param>
    public void TakeDmg(int damge) // ���� �ǰ�
    {
        if(!isDie)
        {
            stat.curHp -= damge;
            Debug.Log("Hit : " + stat.curHp);
            StartCoroutine(Co_isHit());
        }
    }
    #endregion

    #region Coroutine_Function
    protected IEnumerator Co_Think() // ����������� �������� �������ִ� �ڷ�ƾ(���)
    {
        if (!_IsFollow())
        {
            nextMove = Random.Range(-1, 2);
        }
        float nextThinkTime = Random.Range(2f, 5f);
        if (this.gameObject.activeSelf)
        {
            yield return StartCoroutine(Co_StartThinkCoroutineDelay(nextThinkTime));
        }
        else
        {
            yield break;
        }
    }
    protected IEnumerator Co_StartThinkCoroutineDelay(float delay) // Think() �ڷ�ƾ�� ���� �ð� �ڿ� ��������
    {
        yield return new WaitForSeconds(delay);

        StartCoroutine(Co_Think());
    }
    /// <summary>
    /// �ǰ� ���� �� ��¦�Ÿ�
    /// </summary>
    /// <returns></returns>
    protected IEnumerator Co_isHit() // �¾��� �� ����ȯ
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

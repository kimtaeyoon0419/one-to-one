// # System
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;

// # Unity
using UnityEngine;

public enum BossState
{
    spawn,
    fight,
    useSkill,
    usingSkill,
    die
}


[RequireComponent(typeof(Rigidbody2D))]
public abstract class BossMonster : MonoBehaviour
{
    [Header("Stat")]
    [SerializeField] protected int maxHp;
    [SerializeField] protected int curHp;
    [SerializeField] protected int movespeed;
    [SerializeField] protected string bossName;
    [SerializeField] protected BossState state;
    [SerializeField] protected bool isDie;

    [Header("Component")]
    [SerializeField] protected Rigidbody2D rigid;
    [SerializeField] protected Animator animator;

    [Header("Player")]
    [SerializeField] protected GameObject player;
    [SerializeField] protected LayerMask playerLayer;

    [Header("Move")]
    protected Vector2 movePos;
    protected Vector2 velocity;

    [Header("Skill")]
    [SerializeField] protected int skillIndex;
    [SerializeField] protected float skillTrigerTime = 1f;

    [Header("Animation")]
    //protected readonly int hashMove = Animator.StringToHash("IsMove");
    //protected readonly int hashDie = Animator.StringToHash("Die");

    [Header("Diraction")]
    protected int isRight;

    [Header("TakeDamageColor")]
    [SerializeField] protected float delayTime; // WaitForSeconds 값
    SpriteRenderer sr;
    WaitForSeconds waitForSeconds; // 웨잇포세컨드
    Color hafpA = new Color(1, 1, 1, 0.5f); // 피격 색전환 1번 ( 반투명 )
    Color fullA = new Color(1, 1, 1, 1); // 피격 색전환 2번 ( 원본색 )

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        waitForSeconds = new WaitForSeconds(delayTime);
    }

    protected virtual void Start()
    {
        Debug.Log("시작시작시작");
        state = BossState.fight;
        StartCoroutine(SkillTriger(3f));
        player = Physics2D.OverlapCircle(transform.position, 100, playerLayer).gameObject;
        curHp = maxHp;
    }

    /// <summary>
    /// 상태에 따라서 state를 업데이트 해줌
    /// </summary>
    protected virtual void Update()
    {
        switch (state)
        {
            case BossState.spawn:
                break;
            case BossState.fight:
                Move();
                LookPlayer();
                break;
            case BossState.useSkill:
                StartCoroutine(UseSkill());
                break;
            case BossState.usingSkill:

                break;
            case BossState.die:
                if (!isDie)
                    Die();
                break;
        }

        if(curHp <= 0)
        {
            state = BossState.die;
        }
    }

    /// <summary>
    /// 플레이어 바라보는 함수
    /// </summary>
    protected virtual void LookPlayer()
    {
        if (player.transform.position.x - transform.position.x > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
            isRight = 1;
        }
        else if (player.transform.position.x - transform.position.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            isRight = -1;
        }
    }

    #region abstract
    protected abstract void Move();

    protected abstract IEnumerator UseSkill();

    protected abstract IEnumerator SkillTriger(float time);
    #endregion

    public void TakeDamage(int damage)
    {   
        curHp -= damage;
        StartCoroutine(Co_isHit());
        if (curHp <= 0)
        {
            Die();
        }
    }

    protected IEnumerator Co_isHit() // 맞았을 때 색전환
    {
        for (int i = 0; i < 3; i++)
        {
            yield return waitForSeconds;
            sr.color = hafpA;
            yield return waitForSeconds;
            sr.color = fullA;
        }
    }

    protected void Die()
    {
        isDie = true;
        GameManager.instance.curGameState = CurGameState.stageClear;
    }
}

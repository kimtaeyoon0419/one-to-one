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
    protected readonly int hashSkill1 = Animator.StringToHash("Skill1");
    protected readonly int hashSkill2 = Animator.StringToHash("Skill2");
    //protected readonly int hashDie = Animator.StringToHash("Die");

    [Header("Diraction")]
    protected int isRight;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Start()
    {
        StartCoroutine(SkillTriger(3f));
        state = BossState.fight;
        player = Physics2D.OverlapCircle(transform.position, 100, playerLayer).gameObject;
        curHp = maxHp;
    }

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
                int lIndex = Random.Range(0, 10);
                StartCoroutine(UseSkill(lIndex));
                state = BossState.usingSkill;
                break;
            case BossState.usingSkill:

                break;
            case BossState.die:
                if (!isDie)
                    Die();
                break;
        }

    }

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

    protected virtual void Move()
    {
        //Vector2 currentPosition = transform.position;

        //movePos = Vector2.MoveTowards(currentPosition, new Vector2(player.transform.position.x, currentPosition.y), movespeed * Time.deltaTime);

        //// 새로운 위치를 설정합니다
        //transform.position = new Vector2(movePos.x, currentPosition.y);

        velocity.x = movespeed * isRight;
        velocity.y = rigid.velocity.y;

        rigid.velocity = velocity;
    }

    protected abstract IEnumerator UseSkill(int skillNum);

    protected virtual IEnumerator SkillTriger(float time)
    {
        yield return new WaitForSeconds(time);
        if (state == BossState.usingSkill)
        {
            StartCoroutine(SkillTriger(skillTrigerTime));
        }
        else
        {
            state = BossState.useSkill;
        }
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        if(curHp <= 0)
        {
            Die();
        }
    }

    protected abstract void Die();
}

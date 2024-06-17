// # System
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;

// # Unity
using UnityEngine;

public  enum BossState
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
    [SerializeField] private int maxHp;
    [SerializeField] private int curHp;
    [SerializeField] private int movespeed;
    [SerializeField] private string bossName;
    [SerializeField] private BossState state;
    [SerializeField] private bool isDie;

    [Header("Component")]
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Animator animator;

    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private LayerMask playerLayer;

    [Header("Move")]
    private Vector2 movePos;

    [Header("Skill")]
    [SerializeField] private int skillIndex;

    [Header("Animation")]
    private readonly int hashMove = Animator.StringToHash("IsMove");
    private readonly int hashSkill1 = Animator.StringToHash("Skill1");
    private readonly int hashSkill2 = Animator.StringToHash("Skill2");
    private readonly int hashDie = Animator.StringToHash("Die");

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        switch (state)
        {
            case BossState.spawn:
                player = Physics2D.OverlapCircle(transform.position, 100, playerLayer).gameObject;
                break;
            case BossState.fight:
                Move();
                break;
            case BossState.useSkill:
                int lIndex = Random.Range(0, skillIndex);
                StartCoroutine(UseSkill(lIndex));
                state = BossState.usingSkill;
                break;
            case BossState.usingSkill:

                break;
            case BossState.die:
                if(!isDie)
                    Die();
                break;

        }
    }

    protected virtual void Move()
    {
        Vector2 currentPosition = transform.position;

        movePos = Vector2.MoveTowards(currentPosition, new Vector2(player.transform.position.x, currentPosition.y), movespeed * Time.deltaTime);

        // 새로운 위치를 설정합니다
        transform.position = new Vector2(movePos.x, currentPosition.y);
    }

    protected abstract IEnumerator UseSkill(int skillNum);

    protected abstract void Die();
}

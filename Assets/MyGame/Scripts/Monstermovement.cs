using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Monstermovement : MonoBehaviour
{
    public MonsterStat stat;
    public MonsterUnitCode unitCode;

    SpriteRenderer sr;
    Color hafpA = new Color(1, 1, 1, 0.5f);
    Color fullA = new Color(1, 1, 1, 1);

    [SerializeField] private float delayTime;
    private WaitForSeconds waitForSeconds;

    Rigidbody2D rb;
    Animator animator;

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

    }


    void Update()
    {
        Attack();
        if(stat.curAtkSpeed > 0)
        {
            stat.curAtkSpeed -= Time.deltaTime;
        }
    }

    public void TakeDmg(int damge)
    {
        stat.curHp -= damge;
        Debug.Log("아파 내 체력 : " + stat.curHp);
        if(stat.curHp <= 0)
        {   
            gameObject.active = false;
        }
        else StartCoroutine(isHit());
    }
    IEnumerator isHit()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return waitForSeconds;
            sr.color = hafpA;
            yield return waitForSeconds;
            sr.color = fullA;
        }
    }


    private void Attack()//레이케스트에 플레이어가 들어오면 공격!!!
    {
        Debug.DrawRay(rb.position, Vector2.right * stat.atkRange, Color.yellow);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right, stat.atkRange, LayerMask.GetMask("Player"));
        if (hit.collider != null && stat.curAtkSpeed <= 0)
        {
            stat.curAtkSpeed = stat.atkSpeed;
            animator.SetTrigger("Attack");
        }
    }
}

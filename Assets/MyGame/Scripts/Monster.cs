using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    public MonsterStat stat;
    public MonsterUnitCode unitCode;

    SpriteRenderer sr;
    Color hafpA = new Color(1, 1, 1, 0.5f);
    Color fullA = new Color(1, 1, 1, 1);

    [SerializeField] private float delayTime;
    [SerializeField] private GameObject AttackCollider;
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
        AttackRay();
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
        else StartCoroutine(Co_isHit());
    }
    IEnumerator Co_isHit()
    {
        for(int i = 0; i < 3; i++)
        {
            yield return waitForSeconds;
            sr.color = hafpA;
            yield return waitForSeconds;
            sr.color = fullA;
        }
    }


    private void AttackRay()//레이케스트에 플레이어가 들어오면 공격!!!
    {
        Debug.DrawRay(rb.position, Vector2.right * stat.atkRange, Color.yellow);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right, stat.atkRange, LayerMask.GetMask("Player"));
        if (hit.collider != null && stat.curAtkSpeed <= 0)
        {
            stat.curAtkSpeed = stat.atkSpeed;
            animator.SetTrigger("Attack");
        }
    }

    private void Attak()
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

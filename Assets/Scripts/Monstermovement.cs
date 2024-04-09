using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Monstermovement : MonoBehaviour
{
    [SerializeField] int Health = 10;

    SpriteRenderer sr;
    Color hafpA = new Color(255, 0, 0, 0.5f);
    Color fullA = new Color(255, 0, 0, 1);

    [SerializeField] private float delayTime;
    private WaitForSeconds waitForSeconds;

    Rigidbody2D rb;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        waitForSeconds = new WaitForSeconds(delayTime);
    }


    void Update()
    {
        
    }

    public void Hunt(int damge)
    {
        Health -= damge;
        Debug.Log("아파");
        if(Health < 0)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Hunt(PlayerStatManager.instance.AttackPower);
        }
    }

    private void Attack()//레이케스트에 플레이어가 들어오면 공격!!!
    {
        Debug.DrawRay(rb.position, Vector2.right, Color.yellow);
        RaycastHit2D hit = Physics2D.Raycast(rb.position, Vector2.right, 1, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            //공격
        }
    }
}

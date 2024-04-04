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

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        waitForSeconds = new WaitForSeconds(delayTime);
    }


    void Update()
    {
        
    }

    public void Hunt(int damge)
    {
        Health -= damge;
        Debug.Log("¾ÆÆÄ");
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

}

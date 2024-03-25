using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Monstermovement : MonoBehaviour
{
    [SerializeField] int Health = 10;

    void Start()
    {
        
    }


    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Health -= PlayerStatManager.instance.AttackPower;
            Debug.Log(("몬스터 남은 체력 : ") + Health);
        }
    }
}

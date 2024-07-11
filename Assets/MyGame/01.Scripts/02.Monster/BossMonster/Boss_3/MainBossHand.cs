using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBossHand : MonoBehaviour
{
    public MainBoss mainBoss;

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Bullet"))
    //    {
    //        mainBoss.TakeDamage(PlayerStats.attackPower);
    //        Destroy(collision.gameObject);
    //    }
    //}
    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //    if (collision.gameObject.CompareTag("Bullet"))
    //    {
    //        mainBoss.TakeDamage(PlayerStats.attackPower);
    //        Destroy(collision.gameObject);
    //    }
    //}

    public void TkDamage()
    {
              mainBoss.TakeDamage(PlayerStats.attackPower);

    }
}

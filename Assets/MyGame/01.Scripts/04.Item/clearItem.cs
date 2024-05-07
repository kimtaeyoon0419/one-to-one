using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clearItem : MonoBehaviour
{
    public float speed;
    public float jumpPower;
    public int armor;
    public int attack;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerStatManager.instance.StatUP(speed, jumpPower, armor, attack);
        }
    }
}

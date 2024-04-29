using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    private Vector2 JumpPower = new Vector2(1f, 3f);
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        float dir = Random.Range(-1f, 1f);
        rb.velocity = JumpPower;
        rb.velocity = new Vector2(rb.velocity.x * dir, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.instance.PlaySFX("Coin_Get");
            this.gameObject.SetActive(false);
        }
    }
}

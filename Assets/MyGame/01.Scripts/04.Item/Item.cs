// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Item : MonoBehaviour
{
    Rigidbody2D rb;
    private Vector2 JumpPower = new Vector2(1f, 3.5f);
    private bool isPhysics;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 생성 되면 튀어오름
    private void OnEnable()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Player"), true);
        StartCoroutine(Co_WaitCol());
        float dir = Random.Range(-1f, 1f);
        rb.velocity = JumpPower;
        rb.velocity = new Vector2(rb.velocity.x * dir, rb.velocity.y);
        StartCoroutine(velocityReset());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }


    IEnumerator Co_WaitCol()
    {
        yield return new WaitForSeconds(1f);
        isPhysics = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Player"), false);
    }

    IEnumerator velocityReset()
    {
        yield return new WaitForSeconds(0.5f);
        rb.velocity = Vector2.zero;
    }

    #region Unity_Function
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isPhysics == true)
        {
            gameObject.SetActive(false);
        }
    }
    #endregion
}

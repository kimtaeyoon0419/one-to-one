using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSystem : MonoBehaviour
{
    private Vector2 JumpPower = new Vector2(1f, 3f);
    private Rigidbody2D rb;
    private WaitForSeconds waitForSeconds = new WaitForSeconds(1f);

    #region Unity_Function
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // 생성 되면 튀어오름
    private void OnEnable()
    {
        float dir = Random.Range(-1f, 1f);
        rb.velocity = JumpPower;
        rb.velocity = new Vector2(rb.velocity.x * dir , rb.velocity.y);
        StartCoroutine(velocityReset());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // 플레이어랑 부딪히면 코인이 올라가고 효과음 발생
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.instance.coin += 100;
            ObjectPool.ReturnToPool("Coin", gameObject);
            AudioManager.instance.PlaySFX("Coin_Get");
        }
    }
    #endregion

    #region Courutine_Function
    IEnumerator velocityReset()
    {
        yield return waitForSeconds;
        rb.velocity = Vector2.zero;
    }
    #endregion
}

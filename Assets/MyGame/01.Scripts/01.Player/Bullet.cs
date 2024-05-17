using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] float Speed;

    #region Unity_Function
    void OnEnable()
    {   
        AudioManager.instance.PlaySFX("Shot");
        transform.localScale = Vector2.one;
    }

    void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Monster") || collision.CompareTag("Ground"))
        {
            if (collision.CompareTag("Monster")) // 만약 몬스터와 부딪혔다면
            {
                Debug.Log(PlayerStats.attackPower);
                collision.GetComponent<Monster>().TakeDmg(PlayerStats.attackPower);
            }
            ObjectPool.ReturnToPool("Bullet", gameObject);
        }
    }
    #endregion
}

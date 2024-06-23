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
        transform.localScale = Vector2.one;
    }

    void Update()
    {
        transform.Translate(Vector3.right * Speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Monster") || collision.CompareTag("Ground") || collision.CompareTag("Boss"))
        {
            if (collision.CompareTag("Monster")) // ���� ���Ϳ� �ε����ٸ�
            {
                Debug.Log(PlayerStats.attackPower);
                collision.GetComponent<Monster>().TakeDmg(PlayerStats.attackPower);
            }
            if (collision.CompareTag("Boss"))
            {
                collision.GetComponent<BossMonster>().TakeDamage(PlayerStats.attackPower);
            }
            ObjectPool.ReturnToPool("Bullet", gameObject);
        }
    }
    #endregion
}
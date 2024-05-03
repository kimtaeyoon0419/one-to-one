using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
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
        if (collision.CompareTag("Wall") || collision.CompareTag("Monster"))
        {
            if (collision.CompareTag("Monster")) // 만약 몬스터와 부딪혔다면
            {
                collision.GetComponent<Slime>().TakeDmg(PlayerStatManager.instance.AttackPower); // 플레이어스텟매니저의 어택파워만큼 피해를 입힘
            }
            ObjectPool.ReturnToPool("Bullet", gameObject);
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        transform.Translate(Vector3.right * Speed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Monster") || collision.CompareTag("Ground"))
        {
            if (collision.CompareTag("Monster")) // ���� ���Ϳ� �ε����ٸ�
            {
                collision.GetComponent<Monster>().TakeDmg(PlayerStatManager.instance.AttackPower); // �÷��̾�ݸŴ����� �����Ŀ���ŭ ���ظ� ����
            }
            ObjectPool.ReturnToPool("Bullet", gameObject);
        }
    }
    #endregion
}

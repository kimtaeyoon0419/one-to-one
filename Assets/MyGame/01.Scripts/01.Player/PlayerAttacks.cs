using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private Transform AttackPos;

    void Start()
    {
        
    }


    void Update()
    {
        BulletAttack();
    }

    public void BulletAttack()
    {
        if(Input.GetKeyDown(KeyCode.X) && PlayerStatManager.instance.bulletshotCurTime <= 0 && PlayerStatManager.instance.CurBulletCount > 0)
        {
            AudioManager.instance.PlaySFX("Shot");
            PlayerStatManager.instance.CurBulletCount--; // ź �Һ�
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // ���� �ӵ� �ʱ�ȭ
            ObjectPool.instance.Get(0, AttackPos.transform.position, transform.rotation); // Ǯ��
        }
    }
}

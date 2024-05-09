// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] protected Transform attackPos;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region Public_Function
    public void BulletAttack()
    {
        if (Input.GetKeyDown(KeyCode.X) && PlayerStatManager.instance.bulletshotCurTime <= 0 && PlayerStatManager.instance.curBulletCount > 0)
        {
            AudioManager.instance.PlaySFX("Shot"); // �ѼҸ�

            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // ���� �ӵ� �ʱ�ȭ
            PlayerStatManager.instance.UseBullet();
            ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
        }
    }
    #endregion
}

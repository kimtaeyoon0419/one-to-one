using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : WeaponManager
{
    #region Unity_Function
    void Update()
    {
        if (curWeapon != null)
        {
            if (Input.GetKeyDown(KeyCode.X) && PlayerStatManager.instance.bulletshotCurTime <= 0)
            {
                curWeapon();
            }
        }
    }
    #endregion

    #region Public_Function
    public void BulletAttack()
    {
        if(Input.GetKeyDown(KeyCode.X) && PlayerStatManager.instance.bulletshotCurTime <= 0)
        {
            AudioManager.instance.PlaySFX("Shot"); // �ѼҸ�
            
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // ���� �ӵ� �ʱ�ȭ
            PlayerStatManager.instance.UseBullet();
            ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
        }
    }
    #endregion
}

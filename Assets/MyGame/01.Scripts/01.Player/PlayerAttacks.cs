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
            AudioManager.instance.PlaySFX("Shot"); // 총소리
            
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // 공격 속도 초기화
            PlayerStatManager.instance.UseBullet();
            ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
        }
    }
    #endregion
}

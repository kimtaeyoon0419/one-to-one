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
            if (Input.GetKeyDown(KeyCode.X) && bulletshotCurTime <= 0 && stats.bulletCount > 0) // x키 누르면 공격
            {
                curWeapon();
                AudioManager.instance.PlaySFX("Shot");
            }
            else if(stats.bulletCount <= 0)
            {
                DestroyGun();
            }
        }
        if (bulletshotCurTime >= 0)
        {
            bulletshotCurTime -= Time.deltaTime;
        }

        bulletUi.text = ": "+ stats.bulletCount.ToString(); // 탄환 수
    }
    #endregion

    #region Public_Function
    public override void CurWeaponHandGun() 
    {
        base.CurWeaponHandGun();
    }

    public override void CurWeaponRifle()
    {
        base.CurWeaponRifle();
    }

    public override void CurWeaponShotGun()
    {
        base.CurWeaponShotGun();
    }
    #endregion
}

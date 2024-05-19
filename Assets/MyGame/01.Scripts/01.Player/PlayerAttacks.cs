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
            if (Input.GetKeyDown(KeyCode.X) && bulletshotCurTime <= 0) // xŰ ������ ����
            {
                curWeapon();
                AudioManager.instance.PlaySFX("Shot");
            }
        }
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

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
            Debug.Log("총 있음");
            if (Input.GetKeyDown(KeyCode.X) && bulletshotCurTime <= 0 && stats.bulletCount > 0) // x키 누르면 공격
            {
                curWeapon();
                //AudioManager.instance.PlaySFX("Shot");
                Debug.Log("탕탕후루후루");
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

        if (GameManager.instance.curGameStage != CurStage.stage3)
            bulletUi.text = ": " + stats.bulletCount.ToString(); // 탄환 수
        else if (GameManager.instance.curGameStage == CurStage.stage3)
            bulletUi.text = "무한";
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

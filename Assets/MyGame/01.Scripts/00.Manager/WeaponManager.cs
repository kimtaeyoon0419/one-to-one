// # System
using System;
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] protected Transform attackPos;
    [SerializeField] protected Action curWeapon;

    List<float> shootGunRot = new List<float>() { -2f, -1f , 0 , 1f , 2f };
    #region Unity_Function
    
    #endregion
    #region Private_Function
    private void HandGunAttack()
    {
        AudioManager.instance.PlaySFX("Shot"); // 총소리
        PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // 공격 속도 초기화
        PlayerStatManager.instance.UseBullet();
        ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
    }
    private void ShootGunAttack()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i);
            AudioManager.instance.PlaySFX("Shot"); // 총소리
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // 공격 속도 초기화
            PlayerStatManager.instance.UseBullet();

            int rotIndex = i % shootGunRot.Count; // shootGunRot 리스트의 인덱스를 순환하도록 인덱스 계산
            float zRot = shootGunRot[rotIndex]; // shootGunRot 리스트에서 해당 인덱스의 값을 가져옴

            // 플레이어가 바라보는 방향으로 고정된 x와 y 축의 회전 값을 가져와서 z 축의 회전 값만 변경하여 사용
            Quaternion bulletRotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x,
                                                           gameObject.transform.rotation.eulerAngles.y,
                                                           zRot);
            ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, bulletRotation);
        }
    }
    private void RifleGunAttack()
    {
        AudioManager.instance.PlaySFX("Shot"); // 총소리
        PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime * 0.03f; // 공격 속도 초기화
        PlayerStatManager.instance.UseBullet();
        ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
    }
    #endregion
}
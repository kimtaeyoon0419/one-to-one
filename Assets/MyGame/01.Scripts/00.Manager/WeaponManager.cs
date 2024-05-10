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
        AudioManager.instance.PlaySFX("Shot"); // �ѼҸ�
        PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // ���� �ӵ� �ʱ�ȭ
        PlayerStatManager.instance.UseBullet();
        ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
    }
    private void ShootGunAttack()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i);
            AudioManager.instance.PlaySFX("Shot"); // �ѼҸ�
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // ���� �ӵ� �ʱ�ȭ
            PlayerStatManager.instance.UseBullet();

            int rotIndex = i % shootGunRot.Count; // shootGunRot ����Ʈ�� �ε����� ��ȯ�ϵ��� �ε��� ���
            float zRot = shootGunRot[rotIndex]; // shootGunRot ����Ʈ���� �ش� �ε����� ���� ������

            // �÷��̾ �ٶ󺸴� �������� ������ x�� y ���� ȸ�� ���� �����ͼ� z ���� ȸ�� ���� �����Ͽ� ���
            Quaternion bulletRotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x,
                                                           gameObject.transform.rotation.eulerAngles.y,
                                                           zRot);
            ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, bulletRotation);
        }
    }
    private void RifleGunAttack()
    {
        AudioManager.instance.PlaySFX("Shot"); // �ѼҸ�
        PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime * 0.03f; // ���� �ӵ� �ʱ�ȭ
        PlayerStatManager.instance.UseBullet();
        ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
    }
    #endregion
}
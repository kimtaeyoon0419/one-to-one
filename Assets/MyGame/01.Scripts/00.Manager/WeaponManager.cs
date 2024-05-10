// # System
using System;
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] protected Transform attackPos; // �Ѿ� �߻� ��ġ
    [SerializeField] protected Action curWeapon; // ���� ��

    [Header("Gun")]
    [SerializeField] public int handGunBulletCount; // ���� ź��
    [SerializeField] public int rilfeGunBulletCount; // ���� ź��
    [SerializeField] public int shotGunBulletCount; // ���� ź��

    [Header("Rotation List")]
    List<float> shootGunRot = new List<float>() { -2f, -1f , 0 , 1f , 2f }; // ���� ź���� ����
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
    private void ShotGunAttack()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i);
            AudioManager.instance.PlaySFX("Shot"); // �ѼҸ�
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // ���� �ӵ� �ʱ�ȭ

            int rotIndex = i % shootGunRot.Count; // shootGunRot ����Ʈ�� �ε����� ��ȯ�ϵ��� �ε��� ���
            float zRot = shootGunRot[rotIndex]; // shootGunRot ����Ʈ���� �ش� �ε����� ���� ������

            // �÷��̾ �ٶ󺸴� �������� ������ x�� y ���� ȸ�� ���� �����ͼ� z ���� ȸ�� ���� �����Ͽ� ���
            Quaternion bulletRotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x,
                                                           gameObject.transform.rotation.eulerAngles.y,
                                                           zRot);
            ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, bulletRotation);
        }
        PlayerStatManager.instance.UseBullet();
    }
    private void RifleGunAttack()
    {
        AudioManager.instance.PlaySFX("Shot"); // �ѼҸ�
        PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime * 0.03f; // ���� �ӵ� �ʱ�ȭ
        PlayerStatManager.instance.UseBullet();
        ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
    }
    #endregion

    #region Public_Action
    public virtual void CurWeaponHandGun() // ���� ���⸦ �������� ��ü
    {
        if (curWeapon != null)
        {
            curWeapon = () => { };
        }
        PlayerStatManager.instance.maxBublletCount = handGunBulletCount;
        PlayerStatManager.instance.ReloadBullet();
        curWeapon = () => { HandGunAttack(); };  
    }

    public virtual void CurWeaponRifle() // ���� ���⸦ �������� ��ü
    {
        if (curWeapon != null)
        {
            curWeapon = () => { };
        }
        PlayerStatManager.instance.maxBublletCount = rilfeGunBulletCount;
        PlayerStatManager.instance.ReloadBullet();
        curWeapon = () => {RifleGunAttack(); };
    }

    public virtual void CurWeaponShotGun() // ���� ���⸦ �������� ��ü
    {
        if (curWeapon != null)
        {
            curWeapon = () => { };
        }
        PlayerStatManager.instance.maxBublletCount = shotGunBulletCount;
        PlayerStatManager.instance.ReloadBullet();
        curWeapon = () => { ShotGunAttack(); };
    }
    #endregion
}
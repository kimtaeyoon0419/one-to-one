// # System
using System;
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    protected int curBulletCount { get; private set; }
    protected int maxBublletCount;
    protected float bulletshotCoolTime;
    protected float bulletshotCurTime;

    [Header("Attack")]
    [SerializeField] protected Transform attackPos; // �Ѿ� �߻� ��ġ
    [SerializeField] protected Action curWeapon; // ���� ��

    [Header("Gun")]
    [SerializeField] public int handGunBulletCount; // ���� ź��
    [SerializeField] public int rilfeGunBulletCount; // ���� ź��
    [SerializeField] public int shotGunBulletCount; // ���� ź��
    [SerializeField] private float defDistanceRay = 100;
    public LineRenderer lineRenderer;

    [Header("Rotation List")]
    List<float> shootGunRot = new List<float>() { -2f, -1f , 0 , 1f , 2f }; // ���� ź���� ����
    #region Unity_Function
    #endregion
    #region Private_Function
    private void HandGunAttack() // ����
    {
        AudioManager.instance.PlaySFX("Shot"); // �ѼҸ�
        bulletshotCurTime = bulletshotCoolTime; // ���� �ӵ� �ʱ�ȭ
        curBulletCount--;
        ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
    }
    private void ShotGunAttack() // ����
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i);
            AudioManager.instance.PlaySFX("Shot"); // �ѼҸ�
            bulletshotCurTime = bulletshotCoolTime; // ���� �ӵ� �ʱ�ȭ

            int rotIndex = i % shootGunRot.Count; // shootGunRot ����Ʈ�� �ε����� ��ȯ�ϵ��� �ε��� ���
            float zRot = shootGunRot[rotIndex]; // shootGunRot ����Ʈ���� �ش� �ε����� ���� ������

            // �÷��̾ �ٶ󺸴� �������� ������ x�� y ���� ȸ�� ���� �����ͼ� z ���� ȸ�� ���� �����Ͽ� ���
            Quaternion bulletRotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x,
                                                           gameObject.transform.rotation.eulerAngles.y,
                                                           zRot);
            ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, bulletRotation);
        }
        curBulletCount--;
    }
    private void RifleGunAttack() // ����
    {
        AudioManager.instance.PlaySFX("Shot"); // �ѼҸ�
        bulletshotCurTime = bulletshotCoolTime * 0.03f; // ���� �ӵ� �ʱ�ȭ
        curBulletCount--;
        ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
    }

    private void LaserGunAttack()
    {
        if(Physics2D.Raycast(attackPos.position, transform.right * attackPos.rotation.x))
        {
            RaycastHit2D _hit = Physics2D.Raycast(attackPos.position, transform.right * attackPos.rotation.x);
            Draw2Ray(attackPos.position, _hit.point);
        }
        else
        {
            Draw2Ray(attackPos.position, attackPos.transform.right * defDistanceRay);
        }
    }
    private void Draw2Ray(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
    #endregion

    #region Public_Action
    public virtual void CurWeaponHandGun() // ���� ���⸦ �������� ��ü
    {
        if (curWeapon != null)
        {
            curWeapon = () => { };
        }
        maxBublletCount = handGunBulletCount;
        curBulletCount = maxBublletCount;
        curWeapon = () => { HandGunAttack(); };  
    }

    public virtual void CurWeaponRifle() // ���� ���⸦ �������� ��ü
    {
        if (curWeapon != null)
        {
            curWeapon = () => { };
        }
        maxBublletCount = rilfeGunBulletCount;
        curBulletCount = maxBublletCount;
        curWeapon = () => {RifleGunAttack(); };
    }

    public virtual void CurWeaponShotGun() // ���� ���⸦ �������� ��ü
    {
        if (curWeapon != null)
        {
            curWeapon = () => { };
        }
        maxBublletCount = shotGunBulletCount;
        curBulletCount = maxBublletCount;
        curWeapon = () => { ShotGunAttack(); };
    }
    #endregion
}
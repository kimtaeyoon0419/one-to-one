// # System
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

// # Unity
using UnityEngine;

[System.Serializable]
public class Gunstat
{
    public string gunName;

    public int maxBullet;

    public GameObject gun;

    public float bulletShotCool;
}

public class WeaponManager : MonoBehaviour
{
    [SerializeField] protected int curBulletCount { get; private set; }
    protected int maxBublletCount;
    protected float bulletshotCoolTime;
    protected float bulletshotCurTime;

    [Header("GunStat")]
    public Dictionary<string, Gunstat> gunDictionary = new Dictionary<string, Gunstat>();
    public List<Gunstat> gunStatList = new List<Gunstat>();

    [Header("Attack")]
    [SerializeField] protected Transform attackPos; // 총알 발사 위치
    [SerializeField] protected Action curWeapon; // 현재 총

    [Header("Gun")]
    [SerializeField] private float defDistanceRay = 100;
    public LineRenderer lineRenderer;
    [SerializeField] private GameObject curGun;
    [SerializeField] private Transform gunPos;

    [Header("Rotation List")]
    List<float> shootGunRot = new List<float>() { -2f, -1f , 0 , 1f , 2f }; // 샷건 탄퍼짐 방향

    public TextMeshProUGUI bulletUi;
    #region Unity_Function
    private void Start()
    {
        foreach (Gunstat gunstat in gunStatList)
        {
            gunDictionary.Add(gunstat.gunName, gunstat);
        }
    }
    #endregion

    #region Private_Function
    /// <summary>
    /// 총 변경
    /// </summary>
    /// <param name="name">변경할 총의 이름</param>
    private void SetGunStat(string name)
    {
        if (curGun != null)
        {
            Destroy(curGun);
        }

        curGun = gunDictionary[name].gun;
        maxBublletCount = gunDictionary[name].maxBullet;
        bulletshotCoolTime = gunDictionary[name].bulletShotCool;
    }

    /// <summary>
    /// 권총 공격
    /// </summary>
    private void HandGunAttack() // 권총
    {
        AudioManager.instance.PlaySFX("Shot"); // 총소리
        bulletshotCurTime = bulletshotCoolTime; // 공격 속도 초기화
        curBulletCount--;
        ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
    }
    
    /// <summary>
    /// 샷건 공격
    /// </summary>
    private void ShotGunAttack() // 샷건
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log(i);
            AudioManager.instance.PlaySFX("Shot"); // 총소리
            bulletshotCurTime = bulletshotCoolTime; // 공격 속도 초기화

            int rotIndex = i % shootGunRot.Count; // shootGunRot 리스트의 인덱스를 순환하도록 인덱스 계산
            float zRot = shootGunRot[rotIndex]; // shootGunRot 리스트에서 해당 인덱스의 값을 가져옴

            // 플레이어가 바라보는 방향으로 고정된 x와 y 축의 회전 값을 가져와서 z 축의 회전 값만 변경하여 사용
            Quaternion bulletRotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x,
                                                           gameObject.transform.rotation.eulerAngles.y,
                                                           zRot);
            ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, bulletRotation);
        }
        curBulletCount--;
    }

    /// <summary>
    /// 소총 공격
    /// </summary>
    private void RifleGunAttack() // 소총
    {
        AudioManager.instance.PlaySFX("Shot"); // 총소리
        bulletshotCurTime = bulletshotCoolTime * 0.03f; // 공격 속도 초기화
        curBulletCount--;
        ObjectPool.SpawnFromPool("Bullet", attackPos.transform.position, gameObject.transform.rotation);
    }

    //private void LaserGunAttack()
    //{
    //    if(Physics2D.Raycast(attackPos.position, transform.right * attackPos.rotation.x))
    //    {
    //        RaycastHit2D _hit = Physics2D.Raycast(attackPos.position, transform.right * attackPos.rotation.x);
    //        Draw2Ray(attackPos.position, _hit.point);
    //    }
    //    else
    //    {
    //        Draw2Ray(attackPos.position, attackPos.transform.right * defDistanceRay);
    //    }
    //}

    //private void Draw2Ray(Vector2 startPos, Vector2 endPos)
    //{
    //    lineRenderer.SetPosition(0, startPos);
    //    lineRenderer.SetPosition(1, endPos);
    //}
    #endregion

    #region Public_Action
    public virtual void CurWeaponHandGun() // 현재 무기를 권총으로 교체
    {
        if (curWeapon != null)
        {
            curWeapon = () => { };
        }
        SetGunStat("HandGun");
        SetCurGun();

        curBulletCount = maxBublletCount;
        curWeapon = () => { HandGunAttack(); };  
    }

    public virtual void CurWeaponRifle() // 현재 무기를 소총으로 교체
    {
        if (curWeapon != null)
        {
            curWeapon = () => { };
        }
        SetGunStat("Rifle");
        SetCurGun();

        curBulletCount = maxBublletCount;
        curWeapon = () => {RifleGunAttack(); };
    }

    public virtual void CurWeaponShotGun() // 현재 무기를 샷건으로 교체
    {

        if (curWeapon != null)
        {
            curWeapon = () => { };
        }
        SetGunStat("ShotGun");
        SetCurGun();

        curBulletCount = maxBublletCount;
        curWeapon = () => { ShotGunAttack(); };
    }

    /// <summary>
    /// 현재 장착한 총을 들기
    /// </summary>
    public void SetCurGun()
    {
        curGun = Instantiate(curGun, attackPos.position, Quaternion.identity);
        curGun.transform.SetParent(gunPos.transform, false); // 부모 설정, 로컬 위치/회전 유지
        curGun.transform.localPosition = Vector3.zero; // 로컬 위치를 attackPos의 위치로 설정
        curGun.transform.localRotation = Quaternion.identity; // 로컬 회전을 초기화
    }

    /// <summary>
    /// 총알이 없다면 장착한 총 삭제
    /// </summary>
    protected void DestroyGun()
    {
        if(curBulletCount <= 0)
        {
            if(curGun != null)
            {
                Destroy(curGun);
            }
        }
    }
    #endregion
}
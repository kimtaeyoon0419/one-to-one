using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField] private Transform AttackPos;

    void Update()
    {
        BulletAttack();
    }

    public void BulletAttack()
    {
        if(Input.GetKeyDown(KeyCode.X) && PlayerStatManager.instance.bulletshotCurTime <= 0 && PlayerStatManager.instance.CurBulletCount > 0)
        {
            AudioManager.instance.PlaySFX("Shot"); // 총소리
            PlayerStatManager.instance.CurBulletCount--; // 탄 소비
            PlayerStatManager.instance.bulletshotCurTime = PlayerStatManager.instance.bulletshotCoolTime; // 공격 속도 초기화
            ObjectPool.instance.Get(0, AttackPos.transform.position, this.transform.rotation); // 풀링
        }
    }
}

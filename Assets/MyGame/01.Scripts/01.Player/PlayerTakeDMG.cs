using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTakeDMG : MonoBehaviour
{
    [SerializeField] private WaitForSeconds waitForSeconds;
    SpriteRenderer sr;
    Color hafpA = new Color(0, 0, 0);
    Color fullA = new Color(1, 1, 1);
    [SerializeField] private float delayTime = 0.1f;
    [SerializeField] private float hitDelay;
    [SerializeField] private bool isHit = false;
    private CinemachineImpulseSource impulseSource;
    
    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(delayTime);
        sr = GetComponent<SpriteRenderer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void TakeDMG() // 플레이어 갑옷 감소 갑옷이 없다면 사망
    {
        if (PlayerStatManager.instance.ArmorDurability > 0)
        {
            PlayerStatManager.instance.ArmorDurability--;
            StartCoroutine(Co_OnHit());
            StartCoroutine(Co_isHit());
            CameraShakeManager.instance.CameraShake(impulseSource);
        }
        else
        {
            Die();
        }
    }

    IEnumerator Co_OnHit()
    {
        yield return new WaitForSeconds(delayTime * 6);
        isHit = false;
    }

    IEnumerator Co_isHit()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return waitForSeconds;
            sr.color = hafpA;
            yield return waitForSeconds;
            sr.color = fullA;
        }
    }

    private void Die()
    {
        Debug.Log("으앙 주거써");
        PlayerStatManager.instance.isDie = true; // 현재 상태를 죽음으로 바꿈
    }
    //private void OnCollisionEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("EnemyAtk") || collision.CompareTag("Monster")) // 몬스터 공격 범위나 몬스터에 부딪혔다면
    //    {
    //        if (isHit == false)
    //        {
    //            Debug.Log("처 맞음");
    //            isHit = true;
    //            TakeDMG();
    //        }
    //    }
    //}
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAtk") || collision.gameObject.CompareTag("Monster")) // 몬스터 공격 범위나 몬스터에 부딪혔다면
        {
            if (isHit == false)
            {
                Debug.Log("처 맞음");
                isHit = true;
                TakeDMG();
            }
        }
    }
}

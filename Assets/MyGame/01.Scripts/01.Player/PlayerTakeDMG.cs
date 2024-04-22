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

    void TakeDMG()
    {
        if (PlayerStatManager.instance.ArmorDurability > 0)
        {
            PlayerStatManager.instance.ArmorDurability--;
            StartCoroutine(Co_isHit());
            CameraShakeManager.instance.CameraShake(impulseSource);
        }
        else
        {
            Die();
        }
    }

    void HitOn()
    {
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
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAtk"))
        {
            if (isHit == false)
            {
                Debug.Log("처 맞음");
                isHit = true;
                TakeDMG();
                Invoke("HitOn", hitDelay);
            }
        }
    }
}

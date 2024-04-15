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
    [SerializeField] private float hitDelay = 0.2f;
    private CinemachineImpulseSource impulseSource;
    

    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(delayTime);
        sr = GetComponent<SpriteRenderer>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if(hitDelay >= 0)
        {
            hitDelay--;
        }
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
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAtk") && hitDelay <= 0)
        {
            TakeDMG();
        }
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
        Debug.Log("ю╦╬с аж╟е╫А");
    }
}

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

    void TakeDMG() // �÷��̾� ���� ���� ������ ���ٸ� ���
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
        Debug.Log("���� �ְŽ�");
        PlayerStatManager.instance.isDie = true; // ���� ���¸� �������� �ٲ�
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAtk") || collision.CompareTag("Monster")) // ���� ���� ������ ���Ϳ� �ε����ٸ�
        {
            if (isHit == false)
            {
                Debug.Log("ó ����");
                isHit = true;
                TakeDMG();
            }
        }
    }
}

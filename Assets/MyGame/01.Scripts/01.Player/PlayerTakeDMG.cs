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

    void TakeDMG() // ÇÃ·¹ÀÌ¾î °©¿Ê °¨¼Ò °©¿ÊÀÌ ¾ø´Ù¸é »ç¸Á
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
        Debug.Log("À¸¾Ó ÁÖ°Å½á");
        PlayerStatManager.instance.isDie = true; // ÇöÀç »óÅÂ¸¦ Á×À½À¸·Î ¹Ù²Þ
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyAtk") || collision.CompareTag("Monster")) // ¸ó½ºÅÍ °ø°Ý ¹üÀ§³ª ¸ó½ºÅÍ¿¡ ºÎµúÇû´Ù¸é
        {
            if (isHit == false)
            {
                Debug.Log("Ã³ ¸ÂÀ½");
                isHit = true;
                TakeDMG();
            }
        }
    }
}

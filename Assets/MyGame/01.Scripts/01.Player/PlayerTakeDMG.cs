using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTakeDMG : MonoBehaviour
{
    [Header("Component")]
    SpriteRenderer sr;
    [SerializeField] private PlayerStats stat;
    [SerializeField] private Rigidbody2D rb;

    [Header("Color")]
    Color hafpA = new Color(0, 0, 0);
    Color fullA = new Color(1, 1, 1);

    [Header("WaitForSeconds")]
    [SerializeField] private WaitForSeconds waitForSeconds;
    [SerializeField] private float delayTime = 0.1f;

    [Header("isHit")]
    [SerializeField] private bool isHit = false;

    [Header("Camera")]
    private GameObject p_Camera;
    private CameraManager cameraManager;

    [Header("FootPos")]
    [SerializeField] private Transform footPos;

    //public Vector2 test_Vec2;

    #region Unity_Function
    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(delayTime);
        sr = GetComponent<SpriteRenderer>();
        p_Camera = GameObject.FindGameObjectWithTag("Camera");
        cameraManager = p_Camera.GetComponent<CameraManager>();
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("monster"), false); // 몬스터와 충돌무시 해제
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Boss"), false); // 몬스터와 충돌무시 해제
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAtk") || collision.gameObject.CompareTag("Monster")) // 몬스터 공격 범위나 몬스터에 부딪혔다면
        {
            if(footPos.transform.position.y > collision.transform.position.y && rb.velocity.y != 0 )
            {
                collision.gameObject.GetComponent<Monster>().TakeDmg(10000000);
                rb.velocity = Vector2.up * 10f;
            }
            else if (isHit == false)
            {   
                isHit = true;
                TakeDMG();
            }
        }
        if (collision.gameObject.CompareTag("Boss"))
        {
            if(footPos.transform.position.y > collision.transform.position.y && rb.velocity.y != 0)
            {
                collision.gameObject.GetComponent<BossMonster>().TakeDamage(PlayerStats.attackPower);
            }

            if(isHit == false)
            {
                isHit = true;
                TakeDMG();
            }
        }
        if (collision.gameObject.CompareTag("BossAttack"))
        {
            if (isHit == false)
            {
                isHit = true;
                TakeDMG();
            }
        }
    }
    #endregion

    #region Private_Function
    private void TakeDMG() // 플레이어 갑옷 감소 갑옷이 없다면 사망
    {
        if (stat.armorDurability > 0)
        {
            Debug.Log("체력: " +  stat.armorDurability);
            stat.armorDurability--;
            StartCoroutine(Co_OnHit());
            StartCoroutine(Co_isHit());
            cameraManager.OnShakeCamera();
        }
        else
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("monster")); // 몬스터와 충돌무시
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("BossAttack")); // 몬스터와 충돌무시
            Die();
        }
    }

    private void Die()
    {
        GameManager.instance.curGameState = CurGameState.gameOver;
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlaySFX("GameOver");
        Debug.Log("죽음");
    }
    #endregion

    #region Corutine_Function
    IEnumerator Co_OnHit()
    {
        yield return new WaitForSeconds(delayTime * 6);
        isHit = false;
    }

    IEnumerator Co_isHit()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("monster")); // 몬스터와 충돌무시
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("BossAttack")); // 몬스터와 충돌무시
        for (int i = 0; i < 3; i++)
        {
            yield return waitForSeconds;
            sr.color = hafpA;
            yield return waitForSeconds;
            sr.color = fullA;
        }
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("monster"), false); // 몬스터와 충돌무시 해제
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("BossAttack"), false); // 몬스터와 충돌무시 해제
    }
    #endregion
}

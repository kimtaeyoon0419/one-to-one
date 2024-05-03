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
    [SerializeField] private bool isHit = false;
    private GameObject camera;
    private CameraManager cameraManager;

    public Vector2 test_Vec2;

    #region Unity_Function
    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(delayTime);
        sr = GetComponent<SpriteRenderer>();
        camera = GameObject.FindGameObjectWithTag("Camera");
        cameraManager = camera.GetComponent<CameraManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyAtk") || collision.gameObject.CompareTag("Monster")) // ���� ���� ������ ���Ϳ� �ε����ٸ�
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
    private void TakeDMG() // �÷��̾� ���� ���� ������ ���ٸ� ���
    {
        if (PlayerStatManager.instance.ArmorDurability > 0)
        {
            PlayerStatManager.instance.ArmorDurability--;
            StartCoroutine(Co_OnHit());
            StartCoroutine(Co_isHit());
            cameraManager.OnShakeCamera();
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("���� �ְŽ�");
        PlayerStatManager.instance.isDie = true; // ���� ���¸� �������� �ٲ�
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
        for (int i = 0; i < 3; i++)
        {
            yield return waitForSeconds;
            sr.color = hafpA;
            yield return waitForSeconds;
            sr.color = fullA;
        }
    }
    #endregion
}

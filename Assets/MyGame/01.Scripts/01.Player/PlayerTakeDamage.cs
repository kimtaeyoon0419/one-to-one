using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerTakeDamage : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            Debug.Log("�ǰ�");
            TakeDamage();
        }
    }
    private void TakeDamage()
    {
        Debug.Log("����������");
        // �÷��̾� hp �ٿ�����ְ� ������ ü�¹� �ϳ� ����ְ�
        // �¾Ҵٴ� ǥ��( ���� or ȭ�� ��鸲 or �����μ� )
        // �����ڷ�ƾ����
        // �з����¹���

    }
}

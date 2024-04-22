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
            Debug.Log("피격");
            TakeDamage();
        }
    }
    private void TakeDamage()
    {
        Debug.Log("데미지입음");
        // 플레이어 hp 다운시켜주고 오른쪽 체력바 하나 깎아주고
        // 맞았다는 표시( 사운드 or 화면 흔들림 or 광과민성 )
        // 무적코루틴실행
        // 밀려나는물리

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
    [Header("Component")]
    private PlayerAttacks playerAttacks;
    private PlayerMovement playerMovement;

    #region Unity_Function
    private void Awake()
    {
        playerAttacks = GetComponent<PlayerAttacks>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Coin")) // ���ΰ� �ε�������
        {
            CoinManager.instance.coin += 100;
        }
        if (collision.gameObject.CompareTag("Gun")) // �Ѱ� �ε����� ��
        {
            if(collision.gameObject.GetComponent<GunType>().gunType == WaeponList.HandGun) // ������ ȹ���ߴٸ�
            {
                playerAttacks.CurWeaponHandGun();
            }
            if (collision.gameObject.GetComponent<GunType>().gunType == WaeponList.Rifle) // ������ ȹ���ߴٸ�
            {
                playerAttacks.CurWeaponRifle();
            }
            if (collision.gameObject.GetComponent<GunType>().gunType == WaeponList.ShotGun) // ������ ȹ���ߴٸ�
            {
                playerAttacks.CurWeaponShotGun();
            }
            AudioManager.instance.PlaySFX("Gun_Reload");
            collision.gameObject.SetActive(false);
        }
    }
    #endregion
}

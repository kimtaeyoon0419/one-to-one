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
        if (collision.gameObject.CompareTag("Coin")) // ÄÚÀÎ°ú ºÎµúÇûÀ»¶§
        {
            CoinManager.instance.coin += 100;
        }
        if (collision.gameObject.CompareTag("Gun")) // ÃÑ°ú ºÎµúÇûÀ» ¶§
        {
            if(collision.gameObject.GetComponent<GunType>().gunType == WaeponList.HandGun) // ±ÇÃÑÀ» È¹µæÇß´Ù¸é
            {
                playerAttacks.CurWeaponHandGun();
            }
            if (collision.gameObject.GetComponent<GunType>().gunType == WaeponList.Rifle) // ¼ÒÃÑÀ» È¹µæÇß´Ù¸é
            {
                playerAttacks.CurWeaponRifle();
            }
            if (collision.gameObject.GetComponent<GunType>().gunType == WaeponList.ShotGun) // ¼¦°ÇÀ» È¹µæÇß´Ù¸é
            {
                playerAttacks.CurWeaponShotGun();
            }
            AudioManager.instance.PlaySFX("Gun_Reload");
            collision.gameObject.SetActive(false);
        }
    }
    #endregion
}

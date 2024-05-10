using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItem : MonoBehaviour
{
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
        if (collision.gameObject.CompareTag("Coin"))
        {
            CoinManager.instance.coin += 100;
        }
        if (collision.gameObject.CompareTag("Gun"))
        {
            
        }
    }
    #endregion
}

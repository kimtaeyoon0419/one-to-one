// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class Item : MonoBehaviour
{
    #region Unity_Function
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
        }
    }
    #endregion
}

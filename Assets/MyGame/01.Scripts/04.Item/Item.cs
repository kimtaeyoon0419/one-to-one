// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Co_WaitCol());
    }

    IEnumerator Co_WaitCol()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Player"), true);
        yield return new WaitForSeconds(0.5f);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Item"), LayerMask.NameToLayer("Player"), false);
    }


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

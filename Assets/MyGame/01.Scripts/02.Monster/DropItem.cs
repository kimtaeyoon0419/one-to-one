using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    #region Private_Function
    private void DropCoin()
    {
        ObjectPool.SpawnFromPool("Coin", gameObject.transform.position);
    }
    #endregion
}

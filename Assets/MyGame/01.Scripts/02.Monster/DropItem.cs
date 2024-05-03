using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    #region Public_Function
    public void DropCoin()
    {
        ObjectPool.SpawnFromPool("Coin", gameObject.transform.position);
    }
    #endregion
}

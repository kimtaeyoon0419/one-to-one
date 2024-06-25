using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject[] gunList;

    #region Public_Function
    /// <summary>
    /// æ∆¿Ã≈€ ∂≥±∏±‚
    /// </summary>
    public void DropCoin()
    {
        ObjectPool.SpawnFromPool("Coin", gameObject.transform.position);
    }

    /// <summary>
    /// √— ∂≥±∏±‚
    /// </summary>
    public void DropGun()
    {
        int gunDropPercent = Random.Range(0, 5);
        Debug.Log(gunDropPercent);
        if(gunDropPercent == 0)
        {
            Debug.Log("√— ª˝º∫");
            int selectGun = Random.Range(1, gunList.Length);
            Instantiate(gunList[selectGun], gameObject.transform.position, Quaternion.identity);
        }
    }
    #endregion
}

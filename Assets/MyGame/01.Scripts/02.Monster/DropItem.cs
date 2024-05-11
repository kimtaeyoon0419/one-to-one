using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public GameObject[] gunList;

    #region Public_Function
    public void DropCoin()
    {
        ObjectPool.SpawnFromPool("Coin", gameObject.transform.position);
    }
    public void DropGun()
    {
        int gunDropPercent = Random.Range(0, 5);
        Debug.Log(gunDropPercent);
        if(gunDropPercent == 0)
        {
            Debug.Log("ÃÑ »ý¼º");
            int selectGun = Random.Range(0, gunList.Length);
            Instantiate(gunList[selectGun], gameObject.transform.position, Quaternion.identity);
        }
    }
    #endregion
}

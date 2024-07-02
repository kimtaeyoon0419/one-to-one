using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GunList
{
    public string gunName;
    public GameObject gunObj;
}

public class DropItem : MonoBehaviour
{
    public GameObject[] gunList1;
    public List<GunList> gunList;

    private void OnDestroy()
    {
        if (this != null)
        {
            DropCoin();
            DropGun();
        }
    }


    #region Public_Function
    /// <summary>
    /// 아이템 떨구기
    /// </summary>
    public void DropCoin()
    {
        if(ObjectPool.instance != null)
        ObjectPool.SpawnFromPool("Coin", gameObject.transform.position);
    }

    /// <summary>
    /// 총 떨구기
    /// </summary>
    public void DropGun()
    {
        if (gunList == null || gunList.Count == 0)
        {
            Debug.LogError("gunList가 비어 있습니다.");
            return;
        }

        int gunDropPercent = UnityEngine.Random.Range(0, 1); // 0 또는 1을 반환
        Debug.Log(gunDropPercent);
        if (gunDropPercent == 0)
        {
            Debug.Log("총 생성");
            int selectGun = UnityEngine.Random.Range(0, gunList.Count); // 0부터 gunList.Length-1까지 반환
            ObjectPool.SpawnFromPool(gunList[selectGun].gunName, gameObject.transform.position);
        }
    }
    #endregion
}

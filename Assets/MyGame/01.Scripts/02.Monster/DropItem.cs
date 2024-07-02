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
    /// ������ ������
    /// </summary>
    public void DropCoin()
    {
        if(ObjectPool.instance != null)
        ObjectPool.SpawnFromPool("Coin", gameObject.transform.position);
    }

    /// <summary>
    /// �� ������
    /// </summary>
    public void DropGun()
    {
        if (gunList == null || gunList.Count == 0)
        {
            Debug.LogError("gunList�� ��� �ֽ��ϴ�.");
            return;
        }

        int gunDropPercent = UnityEngine.Random.Range(0, 1); // 0 �Ǵ� 1�� ��ȯ
        Debug.Log(gunDropPercent);
        if (gunDropPercent == 0)
        {
            Debug.Log("�� ����");
            int selectGun = UnityEngine.Random.Range(0, gunList.Count); // 0���� gunList.Length-1���� ��ȯ
            ObjectPool.SpawnFromPool(gunList[selectGun].gunName, gameObject.transform.position);
        }
    }
    #endregion
}

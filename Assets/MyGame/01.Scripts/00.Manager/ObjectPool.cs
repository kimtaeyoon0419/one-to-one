using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index, Vector3 posithon, Quaternion rotation)  // Fix : Vector3.rotationd으로 받고 초기화해주기
    {
        GameObject select = null;


        foreach (GameObject Object in pools[index])
        {
            if (!Object.activeSelf)
            {
                select = Object;
                select.transform.position = posithon;
                select.transform.rotation = rotation;
                select.SetActive(true);
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(prefabs[index], posithon, rotation, transform);

            pools[index].Add(select);
        }
        return select;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class test_objcetPool : MonoBehaviour
{
    public static test_objcetPool instance;
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
            select = Instantiate(prefabs[index], posithon, rotation);
            //if (index == 0) select.GetComponent<Bullet>().DirVec(GameManager.instance.player.bulletDirVec);

            pools[index].Add(select);
        }


        return select;
    }
}

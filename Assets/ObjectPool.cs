using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public GameObject[] prefabs;

    List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index, Vector3 posithon, int BulletDiv)
    {
        GameObject select = null;


        foreach (GameObject Object in pools[index])
        {
            if (!Object.activeSelf)
            {
                select = Object;
                select.transform.position = posithon;
                select.SetActive(true);
                break;
            }
        }
        if (!select)
        {
            select = Instantiate(prefabs[index], posithon, Quaternion.identity, transform);
            //if (index == 0) select.GetComponent<Bullet>().DirVec(GameManager.instance.player.bulletDirVec);

            pools[index].Add(select);
        }


        return select;
    }
}

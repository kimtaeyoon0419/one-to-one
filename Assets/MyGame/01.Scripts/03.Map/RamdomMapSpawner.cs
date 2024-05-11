using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamdomMapSpawner : MonoBehaviour
{
    [Header("½ºÆù À§Ä¡")]
    public Transform[] spawnPos;
    public Transform Grid;

    [Header("¸Ê ÇÁ¸®ÆÕ")]
    public GameObject[] mapPerfab;

    public List<int> randomIndex;

    void Start()
    {
        RandomMapSpawn();
    }

    void RandomMapSpawn()
    {
        for (int i = 0; i < mapPerfab.Length; i++)
        {
            randomIndex.Add(i);
        }

        for (int i = 0; i < 5; i++)
        {
            int index = Random.Range(0, randomIndex.Count - 1);
            GameObject instantiatedObject = Instantiate(mapPerfab[randomIndex[index]], spawnPos[i].position, Quaternion.identity);
            instantiatedObject.transform.SetParent(Grid);
            randomIndex.RemoveAt(index);
        }
    }

}

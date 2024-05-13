using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamdomMapSpawner : MonoBehaviour
{
    [Header("스폰 위치")]
    public Transform[] spawnPos;
    public Transform grid;

    [Header("맵 프리팹")]
    public GameObject[] mapPerfab;
    public GameObject bossStageGround;

    private  List<int> randomIndex = new List<int>(); // 맵 개수를 담을 리스트

    void Start()
    {
        RandomMapSpawn();
    }

    void RandomMapSpawn()
    {
        for (int i = 0; i < mapPerfab.Length; i++)
        {
            randomIndex.Add(i); // 맵 개수만큼 리스트에 담음
        }

        for (int i = 0; i < spawnPos.Length; i++)
        {
            int index = Random.Range(0, randomIndex.Count); // 맵 개수만큼 담아놨던 리스트의 길이
            GameObject instantiatedObject = Instantiate(mapPerfab[randomIndex[index]], spawnPos[i].position, Quaternion.identity); // 맵 생성
            instantiatedObject.transform.SetParent(grid); // 생성한 맵의 부모 지정
            randomIndex.RemoveAt(index); // 중복 막기
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            bossStageGround.SetActive(true);
        }
    }
}

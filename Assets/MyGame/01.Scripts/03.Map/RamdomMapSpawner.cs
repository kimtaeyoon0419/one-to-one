using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamdomMapSpawner : MonoBehaviour
{
    [Header("���� ��ġ")]
    public Transform[] spawnPos;
    public Transform grid;

    [Header("�� ������")]
    public GameObject[] mapPerfab;
    public GameObject bossStageGround;

    private  List<int> randomIndex = new List<int>(); // �� ������ ���� ����Ʈ

    void Start()
    {
        RandomMapSpawn();
    }

    void RandomMapSpawn()
    {
        for (int i = 0; i < mapPerfab.Length; i++)
        {
            randomIndex.Add(i); // �� ������ŭ ����Ʈ�� ����
        }

        for (int i = 0; i < spawnPos.Length; i++)
        {
            int index = Random.Range(0, randomIndex.Count); // �� ������ŭ ��Ƴ��� ����Ʈ�� ����
            GameObject instantiatedObject = Instantiate(mapPerfab[randomIndex[index]], spawnPos[i].position, Quaternion.identity); // �� ����
            instantiatedObject.transform.SetParent(grid); // ������ ���� �θ� ����
            randomIndex.RemoveAt(index); // �ߺ� ����
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

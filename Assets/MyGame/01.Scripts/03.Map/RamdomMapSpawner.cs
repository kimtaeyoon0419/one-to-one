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

    [Header("������ ����")]
    public Transform[] itemSpawnPos;
    private bool itemspawned;
    public List<GameObject> clearItem;

    void Start()
    {
        RandomMapSpawn();
    }

    private void Update()
    {
        if(!itemspawned && GameManager.instance.curGameState == CurGameState.stageClear)
        {
            itemspawned = false;
            _SpawnClearItem();
        }
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

    /// <summary>
    /// Ŭ���� ������ ���� 
    /// </summary>
    private void _SpawnClearItem()
    {
        itemspawned = true;
        int itemIndex;
            itemIndex = Random.Range(0, clearItem.Count);
            Instantiate(clearItem[itemIndex], itemSpawnPos[1]);
            clearItem.RemoveAt(itemIndex);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.curGameState = CurGameState.bossSpawn;
        }
    }
}

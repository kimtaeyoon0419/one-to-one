using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamdomMapSpawner : MonoBehaviour
{
    [Header("Component")]
    public BoxCollider2D boxCollider2D;

    [Header("스폰 위치")]
    public Transform[] spawnPos;
    public Transform grid;

    [Header("맵 프리팹")]
    public GameObject[] mapPerfab;
    public GameObject bossStageGround;

    private List<int> randomIndex = new List<int>(); // 맵 개수를 담을 리스트

    [Header("아이템 스폰")]
    public Transform[] itemSpawnPos;
    private bool itemspawned;
    public List<GameObject> clearItem;

    private static List<GameObject> spawnedItems = new List<GameObject>();

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        RandomMapSpawn();
    }

    private void Update()
    {
        if (!itemspawned && GameManager.instance.curGameState == CurGameState.stageClear)
        {
            itemspawned = false;
            _SpawnClearItem();
        }

        if (GameManager.instance.curGameState == CurGameState.fightBoss)
        {
            boxCollider2D.enabled = false;
        }
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

    /// <summary>
    /// 클리어 아이템 스폰 
    /// </summary>
    private void _SpawnClearItem()
    {
        itemspawned = true;
        for (int i = 0; i < 2; i++)
        {
            GameObject item = Instantiate(clearItem[i], itemSpawnPos[i]);
            spawnedItems.Add(item);
        }
    }

    public static void ItemCollected()
    {
        foreach (var item in spawnedItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        spawnedItems.Clear();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.instance.curGameState = CurGameState.bossSpawn;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("BossSpawn")]
    [SerializeField] private bool spawned;
    [SerializeField] private GameObject BossObject;
    [SerializeField] private Transform SpawnPos;

    private void Start()
    {
        BossObject.transform.position = SpawnPos.position;
        BossObject.SetActive(false);
    }

    private void Update()
    {
        if (!spawned)
        {
            if (GameManager.instance.curGameState == CurGameState.bossSpawn)
            {
                BossObject.SetActive(true);
                Debug.Log(BossObject.activeSelf);

                spawned = true;

                GameManager.instance.curGameState = CurGameState.fightBoss;
            }
        }
    }
}

// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class MonsterSpawn : MonoBehaviour
{
    public Transform[] spawnPos;

    private void Start()
    {
        Debug.Log("찾기 시작");
        foreach(var monster in GameManager.instance.monsterType)
        {
            if(monster.stage == GameManager.instance.curStage)
            {
                foreach(Transform t in spawnPos)
                {
                    int r = Random.Range(0, monster.monsters.Length);
                    Instantiate(monster.monsters[r], t.position, Quaternion.identity);
                }
                break;
            }
        }
    }
}

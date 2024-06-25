using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class stageMonsterType
{
    public int stage;

    public GameObject[] monsters;
}

public enum CurGameState
{
    title,
    getReady,
    bossSpawn,
    fightBoss,
    stageClear,
    gameClear,
    gameOver
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string loadingNextScene;

    //public bool stageClear = false;
    //public bool gameOver = false;

    public int selectChar;
    public int curStage;
    public CurGameState curGameState;

    public List<GameObject> clearItem;
    public List<stageMonsterType> monsterType;
    public Transform[] itemSpawnPos;

    [Header("BossSpawned")]
    public bool Stage_1BossSpawned;
    public bool Stage_2BossSpawned;
    public bool Stage_3BossSpawned;

    [Header("NextStage?")]
    public bool nextSceneCheck;

    [Header("Coin")]
    public float coin = 0;

    [Header("ItemSpawned")]
    private bool itemspawned;

    #region Unity_Function
    void Awake()
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
    }

    /// <summary>
    /// 아이템 생성 함수 실행해주는 함수
    /// </summary>
    public void StageClear()
    {
        _SpawnClearItem();
    }
    #endregion

    #region Private_Function
    /// <summary>
    /// 스테이지 클리어하면 아이템 생성
    /// </summary>
    private void _SpawnClearItem()
    {
        int itemIndex;
        for (int i = 0; i < 3; i++)
        {
            itemIndex = Random.Range(0, clearItem.Count);
            Instantiate(clearItem[itemIndex], itemSpawnPos[i]);
            clearItem.RemoveAt(itemIndex);
        }
    }
    #endregion
}

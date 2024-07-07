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

public enum CurStage
{
    title,
    stage1,
    stage2,
    stage3,
    gameClear
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string loadingNextScene;

    //public bool stageClear = false;
    //public bool gameOver = false;

    public int curStage;
    public int selectChar;
    public CurGameState curGameState;
    public CurStage curGameStage;

    public List<stageMonsterType> monsterType;

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

    #endregion

}

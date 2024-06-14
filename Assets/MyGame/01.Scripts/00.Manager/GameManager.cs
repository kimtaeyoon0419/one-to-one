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
    public void StageClear()
    {
        _SpawnClearItem();
    }
    #endregion

    #region Private_Function
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

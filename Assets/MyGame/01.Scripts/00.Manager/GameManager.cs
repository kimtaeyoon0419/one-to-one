using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public string loadingNextScene;

    public bool stageClear = false;
    public bool gameOver = false;

    public List<GameObject> clearItem;
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
    private void Update()
    {
        if(stageClear)
        {
            _SpawnClearItem();
        }
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
        stageClear = false; 
    }
    #endregion
}

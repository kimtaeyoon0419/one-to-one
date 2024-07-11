using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    private void Update()
    {
        if(GameManager.instance.curGameStage == CurStage.gameClear)
        {
            SceneManager.LoadScene("GameClear");
        }
    }
}

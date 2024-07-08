using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChager : MonoBehaviour
{
    public CurStage changeStage;
    public CurGameState changeState;

    private void Start()
    {
        GameManager.instance.curGameStage = changeStage;
        GameManager.instance.curGameState = changeState;
    }
}

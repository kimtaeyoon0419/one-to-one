using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageChager : MonoBehaviour
{
    public CurStage changeStage;

    private void Start()
    {
        GameManager.instance.curGameStage = changeStage;
    }
}

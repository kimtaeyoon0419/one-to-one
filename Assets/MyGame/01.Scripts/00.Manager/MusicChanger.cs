using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    public string chagemusicName;

    #region Unity_Function
    void Start()
    {
        AudioManager.instance.PlayMusic(chagemusicName);
        if(GameManager.instance.curGameStage == CurStage.title)
        {
            GameManager.instance.coin = 0;
        }
    }
    #endregion
}

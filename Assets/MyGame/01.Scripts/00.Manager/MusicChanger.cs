using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    #region Unity_Function
    void Start()
    {
        AudioManager.instance.PlayMusic("Stage_1");
    }
    #endregion
}

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
    }
    #endregion
}

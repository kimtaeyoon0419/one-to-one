using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicChanger : MonoBehaviour
{
    void Start()
    {
        AudioManager.instance.PlayMusic("Stage_1");
    }
}

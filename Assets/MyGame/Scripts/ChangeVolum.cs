using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

public class ChangeVolum : MonoBehaviour
{
    [Header("#Slider")]
    public Slider volumSlider;
    public bool IsBgm;

    void Start()
    {
        
    }

    //private void OnEnable()
    //{
    //    if (IsBgm)
    //    {
    //        volumSlider.value = AudioManager.Instance.bgmPlayer.volume;
    //    }
    //    else
    //    {
    //        volumSlider.value = AudioManager.Instance.sfxPlayers[0].volume;
    //    }
    //}

    //void Update()
    //{

    //}

    //public void ChangeSlidervolum(float volum)
    //{
    //    if (IsBgm)
    //    {
    //        AudioManager.Instance.bgmPlayer.volume = volumSlider.value;
    //    }
    //    else
    //    {
    //        for (int i = 0; i < AudioManager.Instance.sfxPlayers.Length; i++)
    //        {
    //            AudioManager.Instance.sfxPlayers[i].volume = volumSlider.value;
    //        }
    //    }
    //}

}

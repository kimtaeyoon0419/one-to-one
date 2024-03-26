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
        if (IsBgm)
        {
            volumSlider.value = AudioManager.Instance.bgmVolume;
        }
        else
        {
            volumSlider.value = AudioManager.Instance.sfxVolume;
        }
    }

    void Update()
    {
        
    }   

    public void ChangeSlidervolum(float volum)
    {
        if (IsBgm)
        {
            AudioManager.Instance.bgmVolume = volumSlider.value;
        }
        else
        {
            AudioManager.Instance.sfxVolume = volumSlider.value;
        }
    }
    
}

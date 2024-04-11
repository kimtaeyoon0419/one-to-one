using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UiManager : MonoBehaviour
{
    [Header("Setting")]
    public GameObject SettingPanel;
    public Slider musicSlider, sfxSlider;
    [Header("GameOver")]
    public GameObject[] GameOverUI;
    

    private bool SetPanelOnoff = false;
    private bool SetGameOverUI = false;


    void Update()
    {
        
    }

    

    public void SettingPanelOnoff()
    {
        if (SetPanelOnoff == false)
        {
            SetPanelOnoff = true;
            SettingPanel.SetActive(true);
        }
        else
        {
            SetPanelOnoff = false;
            SettingPanel.SetActive(false);
        }
    }

    public void LoadScene(string sceneName)
    {

        if (SetGameOverUI != false)
        {
            for (int i = 0; i < GameOverUI.Length; i++)
            {
                GameOverUI[i].SetActive(false);
            }
            SceneManager.LoadScene(sceneName);
        }
        else SceneManager.LoadScene(sceneName);
    }

    public void ToggleMusic()
    {
        AudioManager.instance.ToggleMusic();
    }
    public void ToggleSFX()
    {
        AudioManager.instance.ToggleSFX();
    }
    public void MusicVolume()
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
    }
    public void SFXVolume()
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }
}

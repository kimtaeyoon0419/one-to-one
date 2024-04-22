using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEditor.Purchasing;

public class UiManager : MonoBehaviour
{
    [Header("Setting")]
    public GameObject SettingPanel;
    public Slider musicSlider, sfxSlider;
    [Header("GameOver")]
    public GameObject[] GameOverUI;
    

    private bool SetPanelOnoff = false;
    private bool SetGameOverUI = false;

    public void SettingPanelOnoff() // ���� �г� on / off
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

    public void LoadScene(string sceneName) // �� �ε�
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

    public void ToggleMusic() // ���� on / off
    {
        AudioManager.instance.ToggleMusic();
    }
    public void ToggleSFX() // ȿ���� on / off
    {
        AudioManager.instance.ToggleSFX();
    }
    public void MusicVolume() // ���� ����
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
    }
    public void SFXVolume() // ȿ���� ����
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }

    public void GameOff() // ���� ����
    {
        Application.Quit();
    }
}

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
    public GameObject GameOverUI;

    private bool SetPanelOnoff = false;

    private void Start()
    {
        if (GameOverUI != null)
        {
            GameOverUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (PlayerStatManager.instance.isDie)
        {
            Gameover();
        }
    }

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

    public static void LoadScene(string sceneName) // �� �ε�
    {
        GameManager.instance.loadingNextScene = sceneName;
        SceneManager.LoadScene("99_LoadingScene");
    }

    public void Gameover()
    {
        if(PlayerStatManager.instance.isDie)
        {
            GameOverUI.SetActive(true);
            PlayerStatManager.instance.isDie = false;
        }
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

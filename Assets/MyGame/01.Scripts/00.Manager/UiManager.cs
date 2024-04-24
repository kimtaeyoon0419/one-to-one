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

    public void SettingPanelToggle() // ���� �г� on / off
    {
            SetPanelOnoff = !SetPanelOnoff;
            SettingPanel.SetActive(SetPanelOnoff);
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
        AudioManager.instance.ToggleMusicMute();
    }
    public void ToggleSFX() // ȿ���� on / off
    {
        AudioManager.instance.ToggleSFXMute();
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEditor.Purchasing;
using System.Security.Cryptography;

public class UiManager : MonoBehaviour
{
    [Header("CharSelect")]
    public GameObject SelectPnl;
    private bool SelectPaneltogge = false;

    [Header("Setting")]
    public GameObject SettingPanel;
    public Slider musicSlider, sfxSlider;
    private bool SetPaneltoggle = false;

    [Header("만든이")]
    public GameObject craftdeveloperPnl;
    private bool craftPnl = false;

    [Header("GameOver")]
    public GameObject GameOverUI;


    #region Unity_Function
    private void Start()
    {
        if (GameOverUI != null)
        {
            GameOverUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (GameManager.instance.gameOver)
        {
            Gameover();
        }
    }
    #endregion

    #region Public_Fuction
    public void SettingPanelToggle() // 설정 패널 on / off
    {
            SetPaneltoggle = !SetPaneltoggle;
            SettingPanel.SetActive(SetPaneltoggle);
    }
    public void CharSelectPanelToggle()
    {
        SelectPaneltogge = !SelectPaneltogge;
        SelectPnl.SetActive(SelectPaneltogge);
    }

    public static void LoadScene(string sceneName) // 씬 로드
    {
        GameManager.instance.loadingNextScene = sceneName;
        SceneManager.LoadScene("99_LoadingScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void credit()
    {
        craftPnl = !craftPnl;
        craftdeveloperPnl.SetActive(craftPnl);
    }

    public void CharSelect(int selectNum)
    {
        GameManager.instance.selectChar = selectNum;
    }

    public void Gameover()
    {
        if(GameManager.instance.gameOver)
        {
            GameOverUI.SetActive(true);
            GameManager.instance.gameOver = false;
        }
    }

    public void ToggleMusic() // 음악 on / off
    {
        AudioManager.instance.ToggleMusicMute();
    }
    public void ToggleSFX() // 효과음 on / off
    {
        AudioManager.instance.ToggleSFXMute();
    }
    public void MusicVolume() // 음악 볼륨
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
    }
    public void SFXVolume() // 효과음 볼륨
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }

    public void GameOff() // 게임 종료
    {
        Application.Quit();
    }
    #endregion
}

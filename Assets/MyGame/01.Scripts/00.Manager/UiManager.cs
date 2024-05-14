using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using UnityEditor.Purchasing;

public class UiManager : MonoBehaviour
{
    [Header("CharSelect")]
    public GameObject SelectPnl;
    private bool SelectPaneltogge = false;

    [Header("Setting")]
    public GameObject SettingPanel;
    public Slider musicSlider, sfxSlider;
    private bool SetPaneltoggle = false;

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
    public void SettingPanelToggle() // ¼³Á¤ ÆÐ³Î on / off
    {
            SetPaneltoggle = !SetPaneltoggle;
            SettingPanel.SetActive(SetPaneltoggle);
    }
    public void CharSelectPanelToggle()
    {
        SelectPaneltogge = !SelectPaneltogge;
        SelectPnl.SetActive(SelectPaneltogge);
    }

    public static void LoadScene(string sceneName) // ¾À ·Îµå
    {
        GameManager.instance.loadingNextScene = sceneName;
        SceneManager.LoadScene("99_LoadingScene");
    }

    public void Gameover()
    {
        if(GameManager.instance.gameOver)
        {
            GameOverUI.SetActive(true);
            GameManager.instance.gameOver = false;
        }
    }

    public void ToggleMusic() // À½¾Ç on / off
    {
        AudioManager.instance.ToggleMusicMute();
    }
    public void ToggleSFX() // È¿°úÀ½ on / off
    {
        AudioManager.instance.ToggleSFXMute();
    }
    public void MusicVolume() // À½¾Ç º¼·ý
    {
        AudioManager.instance.MusicVolume(musicSlider.value);
    }
    public void SFXVolume() // È¿°úÀ½ º¼·ý
    {
        AudioManager.instance.SFXVolume(sfxSlider.value);
    }

    public void GameOff() // °ÔÀÓ Á¾·á
    {
        Application.Quit();
    }
    #endregion
}

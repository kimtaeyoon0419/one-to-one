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

    public void SettingPanelOnoff() // ¼³Á¤ ÆÐ³Î on / off
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

    public static void LoadScene(string sceneName) // ¾À ·Îµå
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

    public void ToggleMusic() // À½¾Ç on / off
    {
        AudioManager.instance.ToggleMusic();
    }
    public void ToggleSFX() // È¿°úÀ½ on / off
    {
        AudioManager.instance.ToggleSFX();
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
}

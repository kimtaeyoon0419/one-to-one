using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class UiManager : MonoBehaviour
{
    public GameObject SettingPanel;
    public GameObject[] GameOverUI;
    public Slider musicSlider, sfxSlider;

    private bool SetPanelOnoff = false;
    private bool SetGameOverUI = false;


    void Update()
    {
        if(PlayerStatManager.instance.CurHp <= 0 && SetGameOverUI == false)
        {
            StartCoroutine(GameOverUIOn());
        }
    }

    IEnumerator GameOverUIOn() // 수정해야함
    {
        SetGameOverUI = true;
        GameOverUI[0].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        GameOverUI[1].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        GameOverUI[2].SetActive(true);
        yield return new WaitForSeconds(0.2f);
        GameOverUI[3].SetActive(true);
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

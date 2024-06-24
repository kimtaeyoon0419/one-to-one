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
    private bool gameOverUiActive = false;
    public GameObject GameOverUI;

    [Header("Fade")]
    public GameObject fadePanel;

    [Header("NextScene?")]
    public GameObject nextSecnePanel;

    [Header("HowToPlay")]
    public GameObject htpPanel;
    private bool htpToggle = false;

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
        if (GameOverUI != null)
        {
            Gameover();
        }
        if(GameManager.instance.nextSceneCheck)
        {
            nextSecnePanel.SetActive(true);
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
        GameManager.instance.curGameState = CurGameState.getReady;
        GameManager.instance.loadingNextScene = sceneName;
        GameManager.instance.nextSceneCheck = false;
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

    public void HowToPlayToggle()
    {
        htpToggle = !htpToggle;
        htpPanel.SetActive(htpToggle);
    }

    public void MoreThink()
    {
        GameManager.instance.nextSceneCheck = false;
        GameManager.instance.curGameState = CurGameState.stageClear;
    }

    public void ReTryButton()
    {
        GameManager.instance.curGameState = CurGameState.getReady;
        LoadScene("Stage_1");
    }

    public void ReturnTitle()
    {
        GameManager.instance.curGameState = CurGameState.title;
        LoadScene("Main");
    }

    public void Gameover()
    {
        if (!GameOverUI.activeSelf && GameManager.instance.curGameState == CurGameState.gameOver)
        {
            GameOverUI.SetActive(true);
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

    public void ButtonSfx()
    {
        AudioManager.instance.PlaySFX("Button");
    }
    #endregion

    #region Fade
    /// <summary>
    /// 밝음 -> 어두움
    /// </summary>
    /// <returns></returns>
    public void FadeIn()
    {
        StartCoroutine(Co_FadeIn());
    }

    /// <summary>
    /// 어두움 -> 밝음
    /// </summary>
    /// <returns></returns>
    public void FadeOut()
    {
        StartCoroutine(Co_FadeOut());
    }

    IEnumerator Co_FadeIn()
    {
        Debug.Log("페이드인");
        fadePanel.SetActive(true);
        Image image = fadePanel.GetComponent<Image>();  
        Color tempColor = image.color;
        while(image.color.a < 1)
        {
            yield return null;
            tempColor.a += Time.deltaTime;
            image.color = tempColor;
            if(tempColor.a >= 1f) tempColor.a = 1f;
        }
        image.color = tempColor;
        fadePanel.SetActive(false);
    }

    IEnumerator Co_FadeOut()
    {
        Debug.Log("페이드 아웃 시작");
        fadePanel.SetActive(true);
        Image image = fadePanel.GetComponent<Image>();
        Color tempColor = image.color;
        while (image.color.a > 0)
        {
            tempColor.a -= Time.deltaTime;
            image.color = tempColor;
            if (tempColor.a <= 0f) tempColor.a = 0f;
            yield return null;
        }
        image.color = tempColor;
        fadePanel.SetActive(false);
    }
    #endregion
}

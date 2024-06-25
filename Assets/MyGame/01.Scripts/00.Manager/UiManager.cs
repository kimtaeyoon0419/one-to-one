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

    [Header("������")]
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
    public void SettingPanelToggle() // ���� �г� on / off
    {
        SetPaneltoggle = !SetPaneltoggle;
        SettingPanel.SetActive(SetPaneltoggle);
    }
    public void CharSelectPanelToggle()
    {
        SelectPaneltogge = !SelectPaneltogge;
        SelectPnl.SetActive(SelectPaneltogge);
    }

    public static void LoadScene(string sceneName) // �� �ε�
    {
        GameManager.instance.curGameState = CurGameState.getReady;
        GameManager.instance.loadingNextScene = sceneName;
        GameManager.instance.nextSceneCheck = false;
        SceneManager.LoadScene("99_LoadingScene");
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// ũ����
    /// </summary>
    public void credit()
    {
        craftPnl = !craftPnl;
        craftdeveloperPnl.SetActive(craftPnl);
    }

    /// <summary>
    /// ĳ���� ����
    /// </summary>
    /// <param name="selectNum">������ ĳ���� �̸�</param>
    public void CharSelect(int selectNum)
    {
        GameManager.instance.selectChar = selectNum;
    }

    /// <summary>
    /// HTP ��ư
    /// </summary>
    public void HowToPlayToggle()
    {
        htpToggle = !htpToggle;
        htpPanel.SetActive(htpToggle);
    }

    /// <summary>
    /// �� �����غ��Կ� ��ư
    /// </summary>
    public void MoreThink()
    {
        GameManager.instance.nextSceneCheck = false;
        GameManager.instance.curGameState = CurGameState.stageClear;
    }

    /// <summary>
    /// ����� ��ư
    /// </summary>
    public void ReTryButton()
    {
        GameManager.instance.curGameState = CurGameState.getReady;
        LoadScene("Stage_1");
    }

    /// <summary>
    /// ����ȭ�� ���ư��� ��ư
    /// </summary>
    public void ReturnTitle()
    {
        GameManager.instance.curGameState = CurGameState.title;
        LoadScene("Main");
    }

    /// <summary>
    /// ���ӿ��� �г� on
    /// </summary>
    public void Gameover()
    {
        if (!GameOverUI.activeSelf && GameManager.instance.curGameState == CurGameState.gameOver)
        {
            GameOverUI.SetActive(true);
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

    public void ButtonSfx() // ��ư ȿ����
    {
        AudioManager.instance.PlaySFX("Button");
    }
    #endregion

    #region Fade
    /// <summary>
    /// ���� -> ��ο�
    /// </summary>
    /// <returns></returns>
    public void FadeIn()
    {
        StartCoroutine(Co_FadeIn());
    }

    /// <summary>
    /// ��ο� -> ����
    /// </summary>
    /// <returns></returns>
    public void FadeOut()
    {
        StartCoroutine(Co_FadeOut());
    }

    IEnumerator Co_FadeIn()
    {
        Debug.Log("���̵���");
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
        Debug.Log("���̵� �ƿ� ����");
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

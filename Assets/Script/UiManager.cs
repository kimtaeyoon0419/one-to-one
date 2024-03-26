using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    public GameObject SettingPanel;

    private bool SetPanelOnoff = false;

    void Start()
    {

    }

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
        SceneManager.LoadScene(sceneName);
        if(sceneName == ("Stage_1"))
        {
            AudioManager.Instance.PlayBGM(true);
        }
    }
}

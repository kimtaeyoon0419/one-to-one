using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("Main");
    }
    public void LoadStage1Scene()
    {
        SceneManager.LoadScene("Stage_1");
    }
}

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
    public void LoadScene(string sceneName)
    {
        
        SceneManager.LoadScene(sceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    public Slider progressbar; // 로딩바
    public TextMeshProUGUI loadingText; // 로딩중 텍스트
    private bool isLoding = false; // 현재 로딩중인가?
    private WaitForSeconds waitForSeconds;
    public float delayTime; // waitForSeconds 딜레이 타임
    private int lodingTextCount = 0; // 로딩 텍스트 카운트

    //operation; <- 비동기식 실행
    //operation.progress; <- 진행률
    //operation.allowSceneActivation; <- 완료되었는가?

    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(delayTime);
    }

    private void Start()
    {
        StartCoroutine(LoadScene());
        StartCoroutine(lodingSet());
    }

    IEnumerator LoadScene()
    {
        yield return null;
        AsyncOperation operation = SceneManager.LoadSceneAsync(GameManager.instance.loadingNextScene);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            yield return null;
            if(progressbar.value < 0.9f) // 진행도에 맞는 slider value 조정
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }
            else if (operation.progress >= 0.9f) // 진행도가 90% 이상이라면 slider value를 최대로
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }
            if (progressbar.value >= 1f)
            {
                isLoding = true;
                StopCoroutine(lodingSet());
                loadingText.text = "Press SpaceBar";
            }

            if(Input.GetKeyDown(KeyCode.Space) && progressbar.value >= 1f)
            {
                operation.allowSceneActivation = true;
            }
        }
    }
    IEnumerator lodingSet()
    {
        while (isLoding == false)
        {
            yield return waitForSeconds;
            switch (lodingTextCount % 3)
            {
                case 0:
                    loadingText.text = "Loding.";
                    break;
                case 1:
                    loadingText.text = "Loding..";
                    break;
                case 2:
                    loadingText.text = "Loding...";
                    break;
            }
            lodingTextCount++;
        }
    }
}

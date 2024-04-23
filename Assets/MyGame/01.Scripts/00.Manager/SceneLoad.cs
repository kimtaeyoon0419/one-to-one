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
    public Slider progressbar; // �ε���
    public TextMeshProUGUI loadingText; // �ε��� �ؽ�Ʈ
    private bool isLoding = false; // ���� �ε����ΰ�?
    private WaitForSeconds waitForSeconds;
    public float delayTime; // waitForSeconds ������ Ÿ��
    private int lodingTextCount = 0; // �ε� �ؽ�Ʈ ī��Ʈ

    //operation; <- �񵿱�� ����
    //operation.progress; <- �����
    //operation.allowSceneActivation; <- �Ϸ�Ǿ��°�?

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
            if(progressbar.value < 0.9f) // ���൵�� �´� slider value ����
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 0.9f, Time.deltaTime);
            }
            else if (operation.progress >= 0.9f) // ���൵�� 90% �̻��̶�� slider value�� �ִ��
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

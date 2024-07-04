// # System
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;

// # Unity
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class portal : MonoBehaviour
{
    [Header("Component")]
    Animator animator;
    CapsuleCollider2D capsuleCollider;
    [SerializeField] private string nextSceneName;
    [SerializeField] private Volume postProcessVolume;
    private LensDistortion lensDistortion;
    private bool isDistortionIncreasing = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        if (postProcessVolume.profile.TryGet<LensDistortion>(out lensDistortion))
        {
            // 초기값 설정
            lensDistortion.intensity.value = 0f;
        }
    }

    private void Update()
    {
        if (GameManager.instance.curGameState == CurGameState.stageClear) // 스테이지 클리어 시 포탈 개방
        {
            animator.SetBool("Open", true);
            capsuleCollider.enabled = true;
            if (GameManager.instance.nextSceneCheck)
            {
                if (!isDistortionIncreasing)
                {
                    StartCoroutine(IncreaseLensDistortion(2f, -1f));
                    GameManager.instance.curGameState = CurGameState.getReady;
                    GameManager.instance.nextSceneCheck = false;
                }
            }
        }
    }

    private IEnumerator IncreaseLensDistortion(float duration, float targetIntensity)
    {
        isDistortionIncreasing = true;
        float startIntensity = lensDistortion.intensity.value;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            lensDistortion.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
            yield return null;
        }

        lensDistortion.intensity.value = targetIntensity;
        SceneManager.LoadScene(nextSceneName);
    }
}

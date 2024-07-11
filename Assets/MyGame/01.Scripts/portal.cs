// # System
using System.Collections;

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
    private bool isDistortionDecreasing = false;

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

    private void Start()
    {
        if (GameManager.instance.curGameStage == CurStage.stage2 || GameManager.instance.curGameStage == CurStage.stage3)
        {
            // 씬이 로드될 때 왜곡을 감소시키는 코루틴 실행
            lensDistortion.intensity.value = -1f; // 최대 왜곡 값으로 초기화
            StartCoroutine(DecreaseLensDistortion(2f, -1f));
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
        if (nextSceneName == "Stage_2")
        {
            GameManager.instance.curGameStage = CurStage.stage2;
        }
        else if (nextSceneName == "Stage_3")
        {
            GameManager.instance.curGameStage = CurStage.stage3;
        }
        SceneManager.LoadScene(nextSceneName);
    }

    private IEnumerator DecreaseLensDistortion(float duration, float maxIntensity)
    {
        isDistortionDecreasing = true;
        lensDistortion.intensity.value = maxIntensity;
        float startIntensity = lensDistortion.intensity.value;
        float targetIntensity = 0f; // 정상적인 화면의 강도
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            lensDistortion.intensity.value = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
            yield return null;
        }

        lensDistortion.intensity.value = targetIntensity;
        isDistortionDecreasing = false;
    }
}

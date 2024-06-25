using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject target; // 카메라가 따라갈 대상
    public float movespeed; // 카메라 이동속도
    private Vector3 targetPosition; // 대상의 현재 위치값

    public BoxCollider2D cameraArea;

    // 박스 콜라이더 영역의 최대, 최소 xyz값을 지님
    private Vector3 minArea;
    private Vector3 maxArea;

    // 카메라의 반너비, 반높이 값을 지닐 변수   
    private float halfWidth;
    private float halfHeight;

    // 카메라의 반높이값을 구할 속성을 이용하기 위한 변수
    private Camera theCamera;

    // 카메라 쉐이크
    private float shakeTime; // 흔들리는 시간
    private float shakeIntensity; // 흔들리는 세기

    [Header("BossCamera")]
    [SerializeField] private Transform BossCameraPos;
    [SerializeField] private float targetSize = 5.7f;
    [SerializeField] private float speed;

    #region Unity_Function
    private void Start()
    {
        theCamera = GetComponent<Camera>();
        minArea = cameraArea.bounds.min; // 카메라 범위 최소값  
        maxArea = cameraArea.bounds.max; // 카메라 범위 최대값
        halfHeight = theCamera.orthographicSize; // 반높이
        halfWidth = halfHeight * Screen.width / Screen.height; // 반너비 공식
    }

    private void Update()
    {
        if(GameManager.instance.curGameState == CurGameState.bossSpawn || GameManager.instance.curGameState == CurGameState.fightBoss || GameManager.instance.curGameState == CurGameState.stageClear)
        {
            theCamera.transform.position = Vector3.Lerp(theCamera.transform.position, BossCameraPos.position, Time.deltaTime * speed);
            theCamera.orthographicSize = Mathf.Lerp(theCamera.orthographicSize, targetSize, Time.deltaTime * speed);
        }

        if (target.gameObject != null && GameManager.instance.curGameState != CurGameState.bossSpawn || GameManager.instance.curGameState != CurGameState.fightBoss)
        {
            targetPosition.Set(this.gameObject.transform.position.x, target.transform.position.y, this.gameObject.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, movespeed * Time.deltaTime); // 카메라의 포지션에서 타겟의 포지션으로 1초에 movespeed만큼 이동

            float clampedX = Mathf.Clamp(this.transform.position.x, minArea.x + halfWidth, maxArea.x - halfWidth); 
            float clampedY = Mathf.Clamp(this.transform.position.y, minArea.y + halfHeight, maxArea.y - halfHeight); // Clamp 공식 ( 벨류 , 최소 , 최대)
                                                          // ( 스크립트를 가지고 있는 오브젝트의 (x or y) 값, 카메라 범위의 (x or y) 최소값 + 카메라 범위 반지름, 카메라 범위의 (x or y) 최대값 - 카메라 범위 반지름 ) 
                                                          // 수정 : 해당 카메라의 x 값이 움직일 필요가 없기에 움직이지 않게 수정
            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }
        

    }
    #endregion

    #region Private_Function
    private IEnumerator ShakeByPos()
    {
        // 흔들리기 직전의 위치 ( 흔들림 종료 후 돌아올 위치 )
        Vector3 startPos = transform.position;

        while (shakeTime > 0.0f)
        {
            // 시작 위치로 부터 구 범위 (Size 1) * shakeIntensity의 범위 안에서 카메라 위치 변동
            transform.position = startPos + Random.insideUnitSphere * shakeIntensity;
            // 시간 감소
            shakeTime -= Time.deltaTime;

            yield return null;
        }
        transform.position = startPos;
    }
    #endregion

    #region Public_Function
    /// <summary>
    /// 카메라 흔들림
    /// </summary>
    /// <param name="shakeTime">흔들리는 시간</param>
    /// <param name="shakeIntensity">흔들리는 범위</param>
    public void OnShakeCamera(float shakeTime = 0.2f, float shakeIntensity  = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine(ShakeByPos());
        StartCoroutine(ShakeByPos());

    }
    #endregion
}

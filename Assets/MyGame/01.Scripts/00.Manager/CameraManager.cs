using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject target; // ī�޶� ���� ���
    public float movespeed; // ī�޶� �̵��ӵ�
    private Vector3 targetPosition; // ����� ���� ��ġ��

    public BoxCollider2D cameraArea;

    // �ڽ� �ݶ��̴� ������ �ִ�, �ּ� xyz���� ����
    private Vector3 minArea;
    private Vector3 maxArea;

    // ī�޶��� �ݳʺ�, �ݳ��� ���� ���� ����   
    private float halfWidth;
    private float halfHeight;

    // ī�޶��� �ݳ��̰��� ���� �Ӽ��� �̿��ϱ� ���� ����
    private Camera theCamera;

    // ī�޶� ����ũ
    private float shakeTime; // ��鸮�� �ð�
    private float shakeIntensity; // ��鸮�� ����

    [Header("BossCamera")]
    [SerializeField] private Transform BossCameraPos;
    [SerializeField] private float targetSize = 5.7f;
    [SerializeField] private float speed;

    #region Unity_Function
    private void Start()
    {
        theCamera = GetComponent<Camera>();
        minArea = cameraArea.bounds.min; // ī�޶� ���� �ּҰ�  
        maxArea = cameraArea.bounds.max; // ī�޶� ���� �ִ밪
        halfHeight = theCamera.orthographicSize; // �ݳ���
        halfWidth = halfHeight * Screen.width / Screen.height; // �ݳʺ� ����
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

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, movespeed * Time.deltaTime); // ī�޶��� �����ǿ��� Ÿ���� ���������� 1�ʿ� movespeed��ŭ �̵�

            float clampedX = Mathf.Clamp(this.transform.position.x, minArea.x + halfWidth, maxArea.x - halfWidth); 
            float clampedY = Mathf.Clamp(this.transform.position.y, minArea.y + halfHeight, maxArea.y - halfHeight); // Clamp ���� ( ���� , �ּ� , �ִ�)
                                                          // ( ��ũ��Ʈ�� ������ �ִ� ������Ʈ�� (x or y) ��, ī�޶� ������ (x or y) �ּҰ� + ī�޶� ���� ������, ī�޶� ������ (x or y) �ִ밪 - ī�޶� ���� ������ ) 
                                                          // ���� : �ش� ī�޶��� x ���� ������ �ʿ䰡 ���⿡ �������� �ʰ� ����
            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }
        

    }
    #endregion

    #region Private_Function
    private IEnumerator ShakeByPos()
    {
        // ��鸮�� ������ ��ġ ( ��鸲 ���� �� ���ƿ� ��ġ )
        Vector3 startPos = transform.position;

        while (shakeTime > 0.0f)
        {
            // ���� ��ġ�� ���� �� ���� (Size 1) * shakeIntensity�� ���� �ȿ��� ī�޶� ��ġ ����
            transform.position = startPos + Random.insideUnitSphere * shakeIntensity;
            // �ð� ����
            shakeTime -= Time.deltaTime;

            yield return null;
        }
        transform.position = startPos;
    }
    #endregion

    #region Public_Function
    /// <summary>
    /// ī�޶� ��鸲
    /// </summary>
    /// <param name="shakeTime">��鸮�� �ð�</param>
    /// <param name="shakeIntensity">��鸮�� ����</param>
    public void OnShakeCamera(float shakeTime = 0.2f, float shakeIntensity  = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine(ShakeByPos());
        StartCoroutine(ShakeByPos());

    }
    #endregion
}

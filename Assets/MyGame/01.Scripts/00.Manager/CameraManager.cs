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
        if (target.gameObject != null)
        {
            targetPosition.Set(this.gameObject.transform.position.x, target.transform.position.y, this.gameObject.transform.position.z);

            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, movespeed * Time.deltaTime); // ī�޶��� �����ǿ��� Ÿ���� ���������� 1�ʿ� movespeed��ŭ �̵�

            float clampedX = Mathf.Clamp(this.transform.position.x, minArea.x + halfWidth, maxArea.x - halfWidth); 
            float clampedY = Mathf.Clamp(this.transform.position.y, minArea.y + halfHeight, maxArea.y - halfHeight); // Clamp ���� ( ���� , �ּ� , �ִ�)
                                                          // ( ��ũ��Ʈ�� ������ �ִ� ������Ʈ�� (x or y) ��, ī�޶� ������ (x or y) �ּҰ� + ī�޶� ���� ������, ī�޶� ������ (x or y) �ִ밪 - ī�޶� ���� ������ ) 
                                                          // ���� : �ش� ī�޶��� x ���� ������ �ʿ䰡 ���⿡ �������� �ʰ� ����
            this.transform.position = new Vector3(clampedX, clampedY, this.transform.position.z);
        }
        if(Input.GetKeyDown(KeyCode.Z))
        {
            OnShakeCamera();
        }
    }

    public void OnShakeCamera(float shakeTime = 0.2f, float shakeIntensity  = 0.1f)
    {
        this.shakeTime = shakeTime;
        this.shakeIntensity = shakeIntensity;

        StopCoroutine("ShakeByPos");
        StartCoroutine("ShakeByPos");

    }

    private IEnumerator ShakeByPos()
    {
        // ��鸮�� ������ ��ġ ( ��鸲 ���� �� ���ƿ� ��ġ )
        Vector3 startPos = transform.position;

        while (shakeTime > 0.0f)
        {
            // ���� ��ġ�� ���� �� ���� (Size 1) * shakeIntensity�� ���� �ȿ��� ī�޶� ��ġ ����
            transform.position= startPos + Random.insideUnitSphere * shakeIntensity;
            // �ð� ����
            shakeTime -= Time.deltaTime;

            yield return null;
        }
        transform.position = startPos;
    }
}
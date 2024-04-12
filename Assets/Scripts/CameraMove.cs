using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public float speed;

    [HideInInspector]
    public Transform CameraLimit;

    public Transform[] Limit;

    float height;
    float width;

    void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
        ChangeLimit(0);
    }

    public void ChangeLimit(int x)
    {
        CameraLimit = Limit[x];
    }

    void LateUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);

        float lx = CameraLimit.localScale.x * 0.5f - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + CameraLimit.position.x, lx + CameraLimit.position.x);

        float ly = CameraLimit.localScale.y * 0.5f - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + CameraLimit.position.y, ly + CameraLimit.position.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }
}

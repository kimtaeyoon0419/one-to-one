using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapMove : MonoBehaviour
{
    public int speed;
    public float delayTime;
    private bool IsMove = false;

    private void Start()
    {
        StartCoroutine(StartMovingAfterDelay());
    }

    private void Update()
    {
        //if (IsMove) MoveMap();
    }

    public void MoveMap()
    {
            transform.Translate(0, -1 * speed * Time.deltaTime, 0);
            IsMove = true;
    }
    IEnumerator StartMovingAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        IsMove = true;
    }
}

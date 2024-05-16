// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class test_laser : MonoBehaviour
{
    [SerializeField] protected Transform attackPos; // 총알 발사 위치

    [SerializeField] private float defDistanceRay = 100;
    public LineRenderer lineRenderer;

    private void Update()
    {
        LaserGunAttack();
    }

    private void LaserGunAttack()
    {
        RaycastHit2D hit = Physics2D.Raycast(attackPos.position, transform.right * attackPos.rotation.x);
        if (hit.collider != null)
        {
            RaycastHit2D _hit = Physics2D.Raycast(attackPos.position, transform.right * attackPos.rotation.x);
            Draw2Ray(attackPos.position, _hit.point);
        }
        else
        {
            Draw2Ray(attackPos.position, attackPos.transform.right * defDistanceRay);
        }
    }
    private void Draw2Ray(Vector2 startPos, Vector2 endPos)
    {
        lineRenderer.SetPosition(0, startPos);
        lineRenderer.SetPosition(1, endPos);
    }
}
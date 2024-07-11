// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class ParticleMove : MonoBehaviour
{
    public ParticleSystem rainParticleSystem; // 비 파티클 시스템
    public Transform cameraTransform; // 카메라 트랜스폼

    private void LateUpdate()
    {
        if (rainParticleSystem != null && cameraTransform != null)
        {
            // 카메라 위치를 기준으로 비 파티클 시스템 위치 설정
            Vector3 newPosition = cameraTransform.position;
            newPosition.y = cameraTransform.position.y + 10f; // 비가 카메라 위에서 내리도록 조정
            newPosition.z = cameraTransform.position.z + 50f;
            rainParticleSystem.transform.position = newPosition;
        }
    }
}

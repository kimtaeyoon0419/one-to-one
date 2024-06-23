using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : MonoBehaviour
{
    [SerializeField] CapsuleCollider2D capsuleCollider2D;

    private void Awake()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    public void ColOn()
    {
        capsuleCollider2D.enabled = true;
    }
}

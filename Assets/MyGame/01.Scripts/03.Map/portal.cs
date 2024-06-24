// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class portal : MonoBehaviour
{
    Animator animator;
    CapsuleCollider2D capsuleCollider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void Update()
    {
        if(GameManager.instance.curGameState == CurGameState.stageClear)
        {
            animator.SetBool("Open", true);
            capsuleCollider.enabled = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float JumpPoawer;

    [SerializeField] float hor;

    bool IsGround = false;
    
    Rigidbody2D rb;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        playerMove();
        playerJump();

        
    }
    void Update()
    {
        
    }

    void playerMove()
    {
        hor = Input.GetAxis("Horizontal");

        rb.velocity = new Vector3(hor * speed, rb.velocity.y);
    }
    void playerJump()
    {
        if (IsGround)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                IsGround = false;
                rb.velocity = Vector2.up * JumpPoawer;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            IsGround = true;
        }
    }
}

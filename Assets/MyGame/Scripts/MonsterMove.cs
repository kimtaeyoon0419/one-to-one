using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    Rigidbody2D rb;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Move(int dir, float speed)
    {
        rb.velocity = new Vector2(rb.velocity.x * dir * speed * Time.deltaTime, rb.velocity.y);
    }
}

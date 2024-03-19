using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float Speed;
    
    void Start()
    {
        
    }

    void Update()
    {
        if (transform.localRotation.y == 0)
        {
            transform.Translate(transform.right * Speed * Time.deltaTime);
        }
        else if (transform.localRotation.y == 180)
        {
            transform.Translate(transform.right * -1  * Speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Well"))
        {
            gameObject.SetActive(false);
        }
    }
}

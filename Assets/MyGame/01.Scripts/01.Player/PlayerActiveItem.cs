using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActiveItem : MonoBehaviour
{
    [SerializeField] GameObject curPotion;
    [SerializeField] private int potionIndex;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && curPotion != null)
        {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Potion"))
        {
            curPotion = collision.gameObject;
            potionIndex = collision.gameObject.GetComponent<Potion>().potionIndex;
        }
    }
}

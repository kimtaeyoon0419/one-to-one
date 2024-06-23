using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashSkill : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField] private float speed;


    private void Update()
    {
        transform.Translate(Vector3.right * -speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}

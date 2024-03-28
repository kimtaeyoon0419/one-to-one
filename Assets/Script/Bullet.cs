using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float Speed;

    public float dirVec;

    void OnEnable()
    {   

        AudioManager.instance.PlaySFX("Shot");
        if (GameManager.instance.player.playerDir == Direction.Right)
        {
            DirVec((int)Direction.Right);
            Debug.Log("¿À");
        }
            
        else
        {
            DirVec((int)Direction.Left);
            Debug.Log("¿Þ");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.right * dirVec * Speed * Time.deltaTime);
    }

    private void DirVec(float dir)
    {
        dirVec = dir;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall") || collision.CompareTag("Monster"))
        {
            gameObject.SetActive(false);
        }
    }
}

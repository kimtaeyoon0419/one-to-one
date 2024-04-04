using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStage : MonoBehaviour
{
    public GameObject BossStageGround;

    void Start()
    {
       
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            BossStageGround.SetActive(true);
        }
    }
}

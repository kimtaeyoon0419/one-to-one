using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletDir : MonoBehaviour
{
    public static PlayerBulletDir instance;

    public PlayerMovement playerMovement;

    private void Awake()
    {
        instance = this; //플레이어 인스턴스화
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletDir : MonoBehaviour
{
    public static PlayerBulletDir instance;

    public PlayerMovement playerMovement;

    private void Awake()
    {
        instance = this; //�÷��̾� �ν��Ͻ�ȭ
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

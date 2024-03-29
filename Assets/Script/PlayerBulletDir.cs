using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletDir : MonoBehaviour
{
    public static PlayerBulletDir instance;

    public Player player;

    private void Awake()
    {
        instance = this; 
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

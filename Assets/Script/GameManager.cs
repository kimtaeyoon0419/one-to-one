using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public ObjectPool pools;
    public Bullet Bullet;
    public MapMove MapMove;

    private bool isMapMove = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
    private void OnEnable()
    {
        Debug.Log("¿¿æ÷");
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}

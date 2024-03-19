using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;

    private List<GameObject> pooledObjects = new List<GameObject>();
    private int amountToPool = 20;

    [SerializeField] private GameObject bulletPrefab;

    public void Awake()
    {
        if(instance != null)
        {
            instance = this;
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        for(int i = 0; i < amountToPool; i++) 
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pooledObjects.Add(obj);
        }
    }
    
    public GameObject getPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        
        return null;
    }

    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    public void DropCoin()
    {
        ObjectPool.instance.Get(1, transform.position, Quaternion.identity); // Ç®¸µÇÏ¿© ÄÚÀÎ ¶³±À
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;

    [SerializeField] GameObject[] players;

    private void Start()
    {
        if (GameManager.instance.selectChar == 1)
        {
            players[0].transform.position = spawnPos.position;
            players[0].gameObject.SetActive(true);
        }
        else if(GameManager.instance.selectChar == 2)
        {
            players[1].transform.position = spawnPos.position;
            players[1].gameObject.SetActive(true);
        }
    }
}

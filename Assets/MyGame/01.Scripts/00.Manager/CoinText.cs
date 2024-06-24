// # System
using System.Collections;
using System.Collections.Generic;
using TMPro;

// # Unity
using UnityEngine;

public class CoinText : MonoBehaviour
{
    public TextMeshProUGUI coinText;

    private void Update()
    {
        coinText.text = "Coin : " + GameManager.instance.coin.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetBulletText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BulletCount;
    public int index;

    void Start()
    {
        
    }

    void Update()
    {
        if (index == 0)
        {
            SetText();
        }

        else if (index == 1)
        {
            SetText22();
        }
    }
    
    public void SetText()
    {
        BulletCount.text = "���� źȯ :" + PlayerStatManager.instance.CurBulletCount;
    }
    public void SetText22()
    {
        BulletCount.text = "ȹ���� ���� :" + CoinManager.instance.coin;
    }
}

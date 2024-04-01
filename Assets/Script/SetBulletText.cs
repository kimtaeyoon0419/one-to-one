using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetBulletText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BulletCount;

    void Start()
    {
        
    }

    void Update()
    {
        SetText();
    }
    
    public void SetText()
    {
        BulletCount.text = "³²Àº ÅºÈ¯ :" + PlayerStatManager.instance.CurBulletCount;
    }
}

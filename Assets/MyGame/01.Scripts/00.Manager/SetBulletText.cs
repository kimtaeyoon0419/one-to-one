using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetBulletText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI BulletCount;
    public int index;

    #region Unity_Function
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
    #endregion

    #region Public_Function
    public void SetText()
    {
        BulletCount.text = "≥≤¿∫ ≈∫»Ø :" + PlayerStatManager.instance.CurBulletCount;
    }
    public void SetText22()
    {
        BulletCount.text = "»πµÊ«— ƒ⁄¿Œ :" + CoinManager.instance.coin;
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.UI;

public class HpBarImg : MonoBehaviour
{
    [SerializeField] private Image[] HpBar;
    private int HpBarNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        HpBarNum = PlayerStatManager.instance.CurHp-1;
    }

    // Update is called once per frame
    void Update()
    {
        if (HpBarNum > PlayerStatManager.instance.CurHp-1)
        {
            HpBar[HpBarNum].gameObject.SetActive(false);
            HpBarNum--;
        }
        else if(HpBarNum < PlayerStatManager.instance.CurHp-1)
        {
            HpBarNum++;
            HpBar[HpBarNum].gameObject.SetActive(true);
        }
        
    }

    public void HpBarActive()
    {
        if (PlayerStatManager.instance.CurHp >=  0)
        {
            HpBar[PlayerStatManager.instance.CurHp].gameObject.SetActive(false);
        }
    }
}

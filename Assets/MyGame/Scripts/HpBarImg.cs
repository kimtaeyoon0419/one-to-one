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
        for(int i = 0; i < PlayerStatManager.instance.CurHp; i++)
        {
            HpBar[i].gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update() // ¼öÁ¤!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    {
        if (HpBarNum > PlayerStatManager.instance.CurHp-1 && 0 > HpBarNum)
        {
            HpBar[HpBarNum].gameObject.SetActive(false);
            HpBarNum--;
        }
        else if(HpBarNum < PlayerStatManager.instance.CurHp-1 && HpBarNum < PlayerStatManager.instance.Max)
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

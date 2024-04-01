using System.Collections;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using UnityEngine;
using UnityEngine.UI;

public class HpBarImg : MonoBehaviour
{
    [SerializeField] private Image[] HpBar;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HpBarActive()
    {
        HpBar[PlayerStatManager.instance.CurHp].gameObject.SetActive(false);
    }
}

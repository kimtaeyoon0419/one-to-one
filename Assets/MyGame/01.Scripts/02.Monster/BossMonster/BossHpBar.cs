using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private BossMonster bossMonster;
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private Slider hpBar;

    private void Start()
    {
        bossName.text = bossMonster.bossName;
    }

    private void Update()
    {
        if (bossMonster != null)
        {
            hpBar.value = (float)bossMonster.curHp / bossMonster.maxHp;
        }
        if (GameManager.instance.curGameState == CurGameState.stageClear)
        {
            gameObject.SetActive(false);
        }
    }
}

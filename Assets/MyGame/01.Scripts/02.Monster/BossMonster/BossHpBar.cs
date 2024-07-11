using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BossHpBar : MonoBehaviour
{
    [SerializeField] private BossMonster bossMonster;
    [SerializeField] private MainBoss mainBoss;
    [SerializeField] private TextMeshProUGUI bossName;
    [SerializeField] private Slider hpBar;

    private void Start()
    {
        if (bossMonster != null)
            bossName.text = bossMonster.bossName;
        else if (mainBoss != null)
            bossName.text = mainBoss.bossName;
    }

    private void Update()
    {
        if (bossMonster != null)
        {
            hpBar.value = (float)bossMonster.curHp / bossMonster.maxHp;
        }
        if (mainBoss != null)
        {
            hpBar.value = (float)mainBoss.curHp / mainBoss.maxHp;
        }
        if (GameManager.instance.curGameState == CurGameState.stageClear)
        {
            gameObject.SetActive(false);
        }
    }
}

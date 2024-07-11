using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBossHand : MonoBehaviour
{
    public MainBoss mainBoss;

    public void TkDamage()
    {
              mainBoss.TakeDamage(PlayerStats.attackPower);
    }
}

// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class SlimeBoss : BossMonster
{
    [Header("Stat")]
    [SerializeField] private float jumpPower;


    #region АјАн
    protected override IEnumerator UseSkill(int skillNum)
    {



        yield return new WaitForSeconds(jumpPower);
    }

    #endregion


    protected override void Die()
    {
        
    }
}

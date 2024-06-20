// # System
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class VampireBoss : BossMonster
{
    [SerializeField] private Transform useMoveSkillPos;
    [SerializeField] private Transform useShotSkillPos;

    protected override IEnumerator UseSkill(int skillNum)
    {



        yield return null;
    }

    protected override void Die()
    {
        
    }
}

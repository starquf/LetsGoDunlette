using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_E_ManaSphere : SkillPiece
{
    public GameObject staticEffectPrefab;

    public override void Cast()
    {
        base.Cast();
        print($"스킬 발동!! 이름 : {PieceName}");

        //StartCoroutine(EffectCast());
    }
}

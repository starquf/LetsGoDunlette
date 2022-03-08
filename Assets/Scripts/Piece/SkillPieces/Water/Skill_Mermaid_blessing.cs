using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Mermaid_blessing : SkillPiece
{


    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {


        onCastEnd?.Invoke();
    }
}

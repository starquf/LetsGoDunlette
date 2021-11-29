using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SkillPiece : RulletPiece
{
    public override int Cast()
    {
        print($"스킬 발동!! 이름 : {PieceName}");
        Camera.main.DOShakePosition(0.5f, 0.3f, 20);

        return value;
    }
}

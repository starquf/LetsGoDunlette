using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPiece : RulletPiece
{
    public override int Cast()
    {
        print($"��ų �ߵ�!! �̸� : {PieceName}");
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        return value;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class Skill_W_Flood : SkillPiece
{
    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        BattleHandler bh = GameManager.Instance.battleHandler;
        ShakeHandler sh = GameManager.Instance.shakeHandler;

        Vector3 rulletPos = bh.mainRullet.transform.position;

        for (int i = 0; i < 10; i++)
        {
            int a = i;

            bh.battleUtil.SetTimer(a * 0.04f, () =>
            {
                animHandler.GetAnim(AnimName.W_WaterIce04)
                    .SetScale(UnityEngine.Random.Range(1.1f, 1.5f))
                    .SetPosition(rulletPos + Vector3.up * 0.35f * (a - 5) + Vector3.right * ((a % 2 == 0) ? 2f : -2f) * UnityEngine.Random.Range(-0.5f, 0.5f))
                    .Play(() =>
                    {
                        if (a == 10 - 1)
                        {
                            onCastEnd?.Invoke();
                        }
                    });

                sh.ShakeBackCvsUI(0.1f + (0.05f * a), 0.1f);
            });
        }


        Flood();
    }

    private void Flood() //물속성을 제외한 모든 조각을 무덤으로 보냄
    {
        List<RulletPiece> rulletPieces = bh.mainRullet.GetPieces();
        for (int i = 0; i < rulletPieces.Count; i++)
        {
            if (rulletPieces[i] != null)
            {
                if (rulletPieces[i].currentType != ElementalType.Water)
                {
                    animHandler.GetAnim(AnimName.W_WaterIce04)
                        .SetScale(1f)
                        .SetPosition(rulletPieces[i].skillIconImg.transform.position)
                        .Play();

                    animHandler.GetTextAnim()
                        .SetPosition(rulletPieces[i].skillIconImg.transform.position)
                        .SetType(TextUpAnimType.Up)
                        .Play("홍수 효과 발동!");

                    bh.battleUtil.SetPieceToGraveyard(i);
                }
            }
        }
    }
}

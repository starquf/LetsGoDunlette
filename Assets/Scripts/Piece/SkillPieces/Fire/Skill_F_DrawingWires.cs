using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_F_DrawingWires : SkillPiece
{
    private BattleHandler bh;

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc().ToString()}");

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(owner.GetComponent<LivingEntity>().AttackPower * 0.3f);

        return attack;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        Rullet rullet = bh.mainRullet;
        List<RulletPiece> pieces = rullet.GetPieces();

        List<SkillPiece> nearSkillPieces = new List<SkillPiece>();

        int nextIdx = GetNearbyIdx(pieces.Count);
        RulletPiece nextPiece = pieces[nextIdx];

        if (nextPiece != null && nextPiece.currentType.Equals(ElementalType.Fire))
        {
            Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
            textEffect.SetType(TextUpAnimType.Up);
            textEffect.transform.position = nextPiece.skillImg.transform.position;
            textEffect.SetScale(0.8f);
            textEffect.Play("도화선 효과발동!");

            animHandler.GetAnim(AnimName.F_ManaSphereHit)
                .SetPosition(nextPiece.skillImg.transform.position)
                .SetScale(0.7f)
                .Play(() => 
                {
                    nextPiece.Cast(target, () =>
                    {
                        CheckLeft(pieces, target, onCastEnd);
                    });

                    bh.battleUtil.SetPieceToGraveyard(nextIdx);
                });
        }
        else
        {
            CheckLeft(pieces, target, onCastEnd);
        }

        animHandler.GetAnim(AnimName.F_ManaSphereHit)
                .SetPosition(target.transform.position)
                .SetScale(0.5f)
                .Play();

        target.GetDamage(GetDamageCalc(), currentType);
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
    }

    private void CheckLeft(List<RulletPiece> pieces, LivingEntity target, Action onCastEnd = null)
    {
        List<SkillPiece> nearSkillPieces = new List<SkillPiece>();

        int prevIdx = GetNearbyIdx(pieces.Count, false);
        RulletPiece prevPiece = pieces[prevIdx];

        if (prevPiece != null && prevPiece.currentType.Equals(ElementalType.Fire))
        {
            Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
            textEffect.SetType(TextUpAnimType.Up);
            textEffect.transform.position = prevPiece.skillImg.transform.position;
            textEffect.SetScale(0.7f);
            textEffect.Play("도화선 효과발동!");

            animHandler.GetAnim(AnimName.F_ManaSphereHit)
                .SetPosition(prevPiece.skillImg.transform.position)
                .SetScale(0.7f)
                .Play(() => 
                {
                    prevPiece.Cast(target, () =>
                    {
                        onCastEnd?.Invoke();
                    });

                    bh.battleUtil.SetPieceToGraveyard(prevIdx);
                });
        }
        else
        {
            onCastEnd?.Invoke();
        }
    }

    private int GetNearbyIdx(int count, bool isNext = true)
    {
        int result = (this.pieceIdx + (isNext ? 1 : -1));

        if (result < 0)
        {
            result = count - 1;
        }
        else if (result >= count)
        {
            result = 0;
        }

        return result;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SI_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();
        if (GameManager.Instance.GetPlayer().cc.IsCC(CCType.Fascinate))
        {
            onCastSkill = SI_Sweet_Voice;
            return pieceInfo[1];
        }
        else
        {
            onCastSkill = SI_Enchanting_Melody;
            return pieceInfo[0];
        }
    }



    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void SI_Enchanting_Melody(LivingEntity target, Action onCastEnd = null) // 플레이어에게 10의 피해를 입힌다. //플레이어에게 5턴간 매혹의 표식을 남긴다. //매혹
    {
        SetIndicator(owner.gameObject, "매혹").OnEnd(() =>
        {
            target.cc.SetCC(CCType.Fascinate, 6);
            target.GetDamage(10, this, owner);

            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

            Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
            hitEffect.transform.position = GameManager.Instance.enemyEffectTrm.position; hitEffect.SetScale(2);


            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });



    }

    private void SI_Sweet_Voice(LivingEntity target, Action onCastEnd = null) //플레이어에게 매혹의 표식이 있다면 룰렛에 있는 플레이어 스킬 중 하나를 세이렌이 사용한다.
    {

        BattleHandler battleHandler = GameManager.Instance.battleHandler;

        Rullet rullet = battleHandler.mainRullet;
        List<RulletPiece> skillPieces = rullet.GetPieces();

        List<SkillPiece> skillPiece = new List<SkillPiece>();
        Dictionary<SkillPiece, int> skillPieceIdxDic = new Dictionary<SkillPiece, int>();

        for (int i = 0; i < skillPieces.Count; i++)
        {
            if (skillPieces[i] == null)
            {
                continue;
            }

            SkillPiece piece = skillPieces[i] as SkillPiece;

            if (piece.isPlayerSkill)
            {
                skillPiece.Add(piece);
                skillPieceIdxDic.Add(piece, i);
            }
        }

        SkillPiece result = null;

        if (skillPiece.Count > 0)
        {
            result = skillPiece[Random.Range(0, skillPiece.Count)];
            result.HighlightColor(0.4f);

            Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
            textEffect.SetType(TextUpAnimType.Damage);
            textEffect.transform.position = result.skillImg.transform.position;
            textEffect.Play($"{result.PieceName} 매혹!");
        }

        Anim_M_Wisp wispEffect = PoolManager.GetItem<Anim_M_Wisp>();
        wispEffect.transform.position = result.skillImg.transform.position;
        wispEffect.SetScale(1.5f);

        wispEffect.Play(() =>
        {
            if (result != null)
            {
                battleHandler.battleUtil.StartCoroutine(battleHandler.battleEvent.ActionEvent(EventTimeSkill.WithSkill, result));

                Inventory temp = result.owner;

                result.owner = this.owner;
                result.Cast(battleHandler.player, () => 
                {
                    result.owner = temp;
                    onCastEnd.Invoke(); 
                });

                battleHandler.battleUtil.SetPieceToGraveyard(skillPieceIdxDic[result]);
            }
            else
            {
                onCastEnd?.Invoke();
            }
        });
    }
}


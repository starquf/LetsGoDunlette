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

    private void SI_Enchanting_Melody(LivingEntity target, Action onCastEnd = null) // �÷��̾�� 10�� ���ظ� ������. //�÷��̾�� 5�ϰ� ��Ȥ�� ǥ���� �����. //��Ȥ
    {
        SetIndicator(owner.gameObject, "��Ȥ").OnEnd(() =>
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

    private void SI_Sweet_Voice(LivingEntity target, Action onCastEnd = null) //�÷��̾�� ��Ȥ�� ǥ���� �ִٸ� �귿�� �ִ� �÷��̾� ��ų �� �ϳ��� ���̷��� ����Ѵ�.
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

            if (skillPieces[i].PieceType.Equals(PieceType.SKILL))
            {
                SkillPiece piece = skillPieces[i] as SkillPiece;
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
            textEffect.Play($"{result.name} ��Ȥ!");
        }

        Vector3 targetPos = target.transform.position;
        targetPos.y -= 0.7f;
        targetPos.x += 0.5f;

        Anim_E_LightningRod lightningRodEffect = PoolManager.GetItem<Anim_E_LightningRod>();
        lightningRodEffect.transform.position = targetPos;

        lightningRodEffect.Play(() =>
        {

            if (result != null)
            {
                battleHandler.battleEvent.OnCastPiece(result);

                result.Cast(battleHandler.player, onCastEnd);

                battleHandler.battleUtil.SetPieceToGraveyard(skillPieceIdxDic[result]);
            }
            else
            {
                onCastEnd?.Invoke();
            }
        });
    }
}


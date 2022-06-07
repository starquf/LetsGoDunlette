using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class SI_Skill : SkillPiece
{
    public Sprite FascinateSpr = null;
    public Gradient effectGradient = null;

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
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[1].GetValue())}");
            onCastSkill = SI_Sweet_Voice;
            return pieceInfo[1];
        }
        else
        {
            onCastSkill = SI_Enchanting_Melody;
            return pieceInfo[0];
        }
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        return desInfos;
    }



    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        onCastSkill(target, onCastEnd);
    }

    private void SI_Enchanting_Melody(LivingEntity target, Action onCastEnd = null) // �÷��̾�� 10�� ���ظ� ������. //�÷��̾�� 5�ϰ� ��Ȥ�� ǥ���� �����. //��Ȥ
    {
        SetIndicator(Owner.gameObject, "��Ȥ").OnEndAction(() =>
        {
            EffectObj effect = PoolManager.GetItem<EffectObj>();
            effect.transform.position = Owner.transform.position;
            effect.SetSprite(FascinateSpr);
            effect.SetColorGradient(effectGradient);
            effect.SetScale(Vector3.one*2);

            effect.Play(GameManager.Instance.enemyEffectTrm.position, () =>
            {
                target.cc.SetCC(CCType.Fascinate, 6);

                GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);

                effect.transform.rotation = Quaternion.Euler(0, 0, 0);

                DOTween.Sequence()
                .Append(effect.transform.DOScale(15, 0.5f))
                .Join(effect.GetComponent<SpriteRenderer>().DOFade(0, 0.5f).SetEase(Ease.InCubic))
                .OnComplete(() =>
                {
                    onCastEnd?.Invoke();
                    effect.EndEffect();
                });
            }, BezierType.Quadratic, isRotate: true, playSpeed: 2.3f);
            
        });
    }

    private void SI_Sweet_Voice(LivingEntity target, Action onCastEnd = null) //�÷��̾�� ��Ȥ�� ǥ���� �ִٸ� �귿�� �ִ� �÷��̾� ��ų �� �ϳ��� ���̷��� ����Ѵ�.
    {
        target.GetDamage(GetDamageCalc(pieceInfo[1].GetValue()), this, Owner);

        Rullet rullet = bh.mainRullet;
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

            animHandler.GetTextAnim()
            .SetType(TextUpAnimType.Up)
            .SetPosition(result.skillIconImg.transform.position)
            .Play($"{result.PieceName} ��Ȥ!");
        }

        if (result != null)
        {
            animHandler.GetAnim(AnimName.M_Wisp).SetPosition(result.skillIconImg.transform.position)
            .SetScale(1.5f)
            .Play(() =>
            {

                bh.battleEvent.StartActionEvent(EventTimeSkill.WithSkill, result);

                Inventory temp = result.Owner;

                result.Owner = Owner;
                result.Cast(bh.player, () =>
                {
                    result.Owner = temp;
                    onCastEnd.Invoke();
                });

                bh.battleUtil.SetPieceToGraveyard(skillPieceIdxDic[result]);
            });

        }
        else
        {
            onCastEnd?.Invoke();
        }
    }
}


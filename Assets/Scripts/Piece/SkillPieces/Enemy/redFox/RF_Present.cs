using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RF_Present : SkillPiece
{
    private Sprite originIcon;

    private BattleHandler bh;

    protected override void Awake()
    {
        base.Awake();

        isPlayerSkill = false;
        originIcon = skillImg.sprite;
    }

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;
    }

    public override void OnRullet()
    {
        bh = GameManager.Instance.battleHandler;

        base.OnRullet();

        List<RulletPiece> pieces = bh.mainRullet.GetPieces();
        List<SkillPiece> targetList = new List<SkillPiece>();

        SkillPiece target = null;

        for (int i = 0; i < pieces.Count; i++)
        {
            SkillPiece piece = pieces[i] as SkillPiece;

            if (piece != null && piece.isPlayerSkill)
            {
                targetList.Add(piece);
            }
        }

        if (targetList.Count > 0)
        {
            target = targetList[Random.Range(0, targetList.Count)];

            skillImg.sprite = target.skillImg.sprite;
        }
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        skillImg.sprite = originIcon;

        SetIndicator(owner.gameObject, "����").OnEnd(() =>
        {
            target.GetDamage(Value, this, owner);
            GameManager.Instance.shakeHandler.ShakeBackCvsUI(2f, 0.2f);
            Anim_M_Sword hitEffect = PoolManager.GetItem<Anim_M_Sword>();
            hitEffect.transform.position = owner.transform.position;

            hitEffect.Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }
}

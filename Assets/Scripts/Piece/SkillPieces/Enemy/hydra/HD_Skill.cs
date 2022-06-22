using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class HD_Skill : SkillPiece
{
    private GameObject skill_poison;

    private InventoryHandler ih;

    protected override void Awake()
    {
        base.Awake();

        isPlayerSkill = false;
    }

    protected override void Start()
    {
        base.Start();

        skill_poison = GameManager.Instance.skillContainer.GetSkillPrefab<HD_Poison>();

        bh = GameManager.Instance.battleHandler;
        ih = GameManager.Instance.inventoryHandler;
    }

    public override PieceInfo ChoiceSkill()
    {
        base.ChoiceSkill();

        if (Random.Range(0, 100) < value)
        {
            onCastSkill = HD_Tail_Swing;

            usedIcons.Add(DesIconType.Attack);

            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo[0].GetValue())}");
            return pieceInfo[0];
        }
        else
        {
            onCastSkill = HD_Poison_Spray;
            return pieceInfo[1];
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

    private void HD_Tail_Swing(LivingEntity target, Action onCastEnd = null)
    {
        SetIndicator(Owner.gameObject, "공격").OnEndAction(() =>
        {
            target.GetDamage(GetDamageCalc(pieceInfo[0].GetValue()), this, Owner);

            animHandler.GetAnim(AnimName.M_Butt).SetPosition(GameManager.Instance.enemyEffectTrm.position)
            .SetScale(2)
            .Play(() =>
            {
                onCastEnd?.Invoke();
            });
        });
    }

    private void HD_Poison_Spray(LivingEntity target, Action onCastEnd = null) // 조각을 2개 추가한다.
    {
        SetIndicator(Owner.gameObject, "조각 강제이동").OnEndAction(() =>
        {
            Rullet rullet = bh.mainRullet;
            List<RulletPiece> pieces = rullet.GetPieces();

            List<int> pieceIdxs = new List<int>() { 0, 1, 2, 3, 4, 5 };

            for (int i = 0; i < 2; i++)
            {
                int a = i;

                int randIdx = 0;
                int pieceIdx = 0;
                bool stop = false;

                do
                {
                    if (pieceIdxs.Count <= 0)
                    {
                        stop = true;
                        break;
                    }

                    randIdx = Random.Range(0, pieceIdxs.Count);
                    pieceIdx = pieceIdxs[randIdx];

                    pieceIdxs.RemoveAt(randIdx);
                }
                while (pieces[pieceIdx] == null || pieces[pieceIdx] == this);

                if (stop)
                {
                    break;
                }

                Vector3 effectPos = pieces[pieceIdx].skillIconImg.transform.position;

                animHandler.GetTextAnim()
                .SetType(TextUpAnimType.Up)
                .SetPosition(effectPos)
                .SetScale(0.8f)
                .Play("독분사 효과발동!");

                GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.25f);

                animHandler.GetAnim(AnimName.N_PoisionCloud)
                    .SetPosition(effectPos)
                    .SetScale(0.7f)
                    .Play();

                bh.battleUtil.SetPieceToGraveyard(pieceIdx);
            }

            SetIndicator(Owner.gameObject, "조각 추가").OnEndAction(() =>
            {
                animHandler.GetAnim(AnimName.SkillEffect01)
                .SetPosition(Owner.transform.position)
                .SetScale(2f)
                .SetRotation(Vector3.forward * -90f)
                .Play(() =>
                {
                    for (int i = 0; i < 2; i++)
                    {
                        bh.battleUtil.SetTimer(0.25f * i, () => { ih.CreateSkill(skill_poison, Owner, Owner.transform.position); });
                    }

                    bh.battleUtil.SetTimer(0.2f, onCastEnd);
                });
            });
        });
    }
}

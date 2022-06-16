using System;
using System.Collections.Generic;
using UnityEngine;

public class Taros : MonoBehaviour
{
    private Action<SkillPiece, Action> skillEvent = null;
    private SkillEvent skillEventInfo = null;

    private Inventory owner;

    public int patrolCount = 0;

    public List<DesIconInfo> desInfos = new List<DesIconInfo>();
    public PieceInfo pieceInfo;
    public SkillPiece ta_Skill;

    // Start is called before the first frame update
    private void Awake()
    {
        owner = GetComponent<Inventory>();
    }

    public void TarosSkill()
    {
        GameManager.Instance.battleHandler.battleEvent.RemoveEventInfo(skillEventInfo);

        skillEvent = (sp, action) =>
        {
            patrolCount--;
            if (patrolCount < 0)
            {
                patrolCount = 3;
            }

            if (sp.Owner == owner) // 발동된 스킬이 타로스의 스킬이라면
            {
                action?.Invoke();
                return;
            }

            if (owner.GetComponent<EnemyHealth>().IsDie || GameManager.Instance.battleHandler.player.IsDie)
            {
                action?.Invoke();
                return;
            }

            if (sp.isTargeting == false)
            {
                action?.Invoke();
                return;
            }

            if (patrolCount <= 0)
            {
                action?.Invoke();
                return;
            }

            EnemyIndicator indi = owner.GetComponent<EnemyIndicator>();
            indi.HideText();
            desInfos[0].SetInfo(DesIconType.Attack, $"{GetDamageCalc(pieceInfo.GetValue())}");
            indi.ShowText("접근 금지", () =>
            GameManager.Instance.battleHandler.castUIHandler.ShowCasting(pieceInfo, desInfos, ta_Skill, () =>
            {
                GameManager.Instance.GetPlayer().GetDamage(GetDamageCalc(pieceInfo.GetValue()), owner);
                GameManager.Instance.animHandler.GetAnim(AnimName.M_Sword)
                .SetPosition(GameManager.Instance.enemyEffectTrm.position)
                .SetScale(2)
                .Play(() =>
                {
                    GameManager.Instance.battleHandler.battleUtil.SetTimer(0.5f + 0.25f, () =>
                    {
                        GameManager.Instance.battleHandler.castUIHandler.ShowPanel(false, false);
                        action?.Invoke();
                    });
                });
            }));
        };

        skillEventInfo = new SkillEvent(EventTimeSkill.AfterSkill, skillEvent);
        GameManager.Instance.battleHandler.battleEvent.BookEvent(skillEventInfo);
    }

    public int GetDamageCalc(int addValue) //Player 전용임
    {
        int attack = owner.GetComponent<LivingEntity>().AttackPower + addValue;
        return attack;
    }
}

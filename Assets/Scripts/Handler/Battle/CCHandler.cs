using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCHandler : MonoBehaviour
{
    private BattleHandler battleHandler;

    // 전투에 참여하는 모든 LivingEntity의 CC를 관리해주는 핸들러
    private List<CrowdControl> crowdControls = new List<CrowdControl>();

    private void Start()
    {
        battleHandler = GameManager.Instance.battleHandler;
    }

    public void Init()
    {
        List<CrowdControl> ccList = new List<CrowdControl>();
        ccList.Add(battleHandler.player.cc);

        for (int i = 0; i < battleHandler.enemys.Count; i++)
        {
            ccList.Add(battleHandler.enemys[i].cc);
        }

        crowdControls = ccList;
    }

    public void DecreaseCC()
    {
        Init();

        for (int i = 0; i < crowdControls.Count; i++)
        {
            crowdControls[i].DecreaseAllTurn();
        }
    }

    public void CheckCC(CCType ccType)
    {
        Init();

        switch (ccType)
        {
            case CCType.Stun:
                CheckType(ccType, Stun);
                break;
            case CCType.Wound:
                CheckType(ccType, Wound);
                break;
        }
    }

    private void CheckType(CCType ccType, Action<CrowdControl> action)
    {
        for (int i = 0; i < crowdControls.Count; i++)
        {
            if (crowdControls[i].GetComponent<LivingEntity>().IsDie) continue;

            if (crowdControls[i].ccDic[ccType] > 0)
            {
                action?.Invoke(crowdControls[i]);
            }
        }
    }

    private void Wound(CrowdControl target)
    {
        target.GetComponent<LivingEntity>().GetDamage(target.ccDic[CCType.Wound]);
    }

    #region Stun


    private void Stun(CrowdControl target)
    {
        Inventory Owner = target.GetComponent<Inventory>();

        List<RulletPiece> pieces = battleHandler.mainRullet.GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            // 빈칸이라면
            if (pieces[i] == null) continue;

            // 룰렛 안에 있는 스킬이 대상의 스킬이라면
            if ((pieces[i] as SkillPiece).Owner == Owner)
            {
                battleHandler.mainRullet.PutRulletPieceToGraveYard(i);
            }
        }
    }

    #endregion
}

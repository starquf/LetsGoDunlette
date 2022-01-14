using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCHandler : MonoBehaviour
{
    private BattleHandler battleHandler;

    // ������ �����ϴ� ��� LivingEntity�� CC�� �������ִ� �ڵ鷯
    private List<CrowdControl> crowdControls = new List<CrowdControl>();

    private void Start()
    {
        battleHandler = GameManager.Instance.battleHandler;
    }

    public void Init(List<CrowdControl> ccList)
    {
        crowdControls = ccList;
    }

    public void DecreaseCC()
    {
        for (int i = 0; i < crowdControls.Count; i++)
        {
            crowdControls[i].DecreaseAllTurn();
        }
    }

    public void CheckCC(CCType ccType)
    {
        switch (ccType)
        {
            case CCType.Stun:
                CheckStun();
                break;
        }
    }

    #region Stun

    private void CheckStun()
    {
        for (int i = 0; i < crowdControls.Count; i++)
        {
            // �����Ǿ��ִٸ�
            if (crowdControls[i].ccDic[CCType.Stun] > 0)
            {
                Stun(crowdControls[i]);
            }
        }
    }

    private void Stun(CrowdControl target)
    {
        Inventory owner = target.GetComponent<Inventory>();

        List<RulletPiece> pieces = battleHandler.mainRullet.GetPieces();

        for (int i = 0; i < pieces.Count; i++)
        {
            // ��ĭ�̶��
            if (pieces[i] == null) continue;

            // �귿 �ȿ� �ִ� ��ų�� ����� ��ų�̶��
            if ((pieces[i] as SkillPiece).owner == owner)
            {
                battleHandler.mainRullet.PutRulletPieceToGraveYard(i);
            }
        }
    }

    #endregion
}

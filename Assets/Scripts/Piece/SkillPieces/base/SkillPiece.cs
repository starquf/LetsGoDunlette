using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillPiece : RulletPiece
{
    public bool isPlayerSkill = true;
    public bool isInRullet = false;

    public bool isDisposable = false; //1ȸ���ΰ�
    public bool isRandomSkill = false; //���� ��ų�ΰ�??

    public bool hasTarget = true;

    public PieceInfo[] pieceInfo;
    protected Action<LivingEntity, Action> onCastSkill;

    protected List<DesIconInfo> desInfos = new List<DesIconInfo>();

    [HideInInspector]
    public Inventory owner;

    protected override void Awake()
    {
        base.Awake();

        PieceType = PieceType.SKILL;

        for (int i = 0; i < 3; i++)
        {
            desInfos.Add(new DesIconInfo());
        }
    }

    public virtual PieceInfo ChoiceSkill()
    {
        return new PieceInfo("None","None");
    }


    public override void Cast(LivingEntity targetTrm, Action onCastEnd = null)
    {

    }

    public virtual string GetPieceDes()
    {
        string des = PieceDes;
        des.Replace("{ownerDmg}", owner.GetComponent<LivingEntity>().AttackPower.ToString());

        return PieceDes;
    }

    public virtual List<DesIconInfo> GetDesIconInfo()
    {
        for (int i = 0; i < desInfos.Count; i++)
        {
            desInfos[i].iconType = DesIconType.None;
        }

        return desInfos;
    }

    public virtual bool CheckSilence()
    {
        CrowdControl cc = owner.GetComponent<CrowdControl>();

        // ħ�� �����ΰ�?
        return cc.ccDic[CCType.Silence] > 0;
    }

    private GameObject go;
    private string content;
    protected SkillPiece SetIndicator(GameObject go, string content)
    {
        this.go = go;
        this.content = content;

        return this;
    }

    public void OnEnd(Action action)
    {
        EnemyIndicator enemyIndicator = go.GetComponent<EnemyIndicator>();
        if (enemyIndicator != null)
        {
            enemyIndicator.ShowText(content,action);
        }
        else
        {
            //print("enemyIndicator is null!");
        }
    }

}

using System;
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


    [HideInInspector]
    public Inventory owner;

    protected override void Awake()
    {
        base.Awake();

        PieceType = PieceType.SKILL;
    }

    public virtual PieceInfo ChoiceSkill()
    {
        return new PieceInfo("None","None");
    }


    public override void Cast(LivingEntity targetTrm, Action onCastEnd = null)
    {

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

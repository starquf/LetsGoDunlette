using System;
using UnityEngine;

public class SkillPiece : RulletPiece
{
    public bool isPlayerSkill = true;
    public bool isInRullet = false;

    public bool isChained = false;
    public bool isDisposable = false; //1회용인가

    [HideInInspector]
    public Inventory owner;

    protected override void Awake()
    {
        base.Awake();

        PieceType = PieceType.SKILL;
    }


    public override void Cast(LivingEntity targetTrm, Action onCastEnd = null)
    {

    }

    public virtual bool CheckSilence()
    {
        CrowdControl cc = owner.GetComponent<CrowdControl>();

        // 침묵 상태인가?
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

    public void OnComplete(Action action)
    {
        EnemyIndicator enemyIndicator = go.GetComponent<EnemyIndicator>();
        if (enemyIndicator != null)
        {
            enemyIndicator.ShowText(content,action);
        }
        else
        {
            print("enemyIndicator is null!");
        }
    }

}

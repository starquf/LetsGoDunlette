using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillPiece : RulletPiece
{
    [Header("스킬 세팅")]
    public bool isPlayerSkill = true;                 // 플레이어 스킬인가?
    public bool isTargeting = true;                   // 타게팅스킬인가?
    public bool isRandomSkill = false;                // 랜덤 스킬인가??
    public bool isDisposable = false;                 // 1회용인가
    public SkillRange skillRange = SkillRange.Single; // 스킬의 공격 범위
    public GradeInfo skillGrade = GradeInfo.Normal;
    public List<DesIconType> usedIcons = new List<DesIconType>();

    public List<PieceInfo> pieceInfo;
    protected Action<LivingEntity, Action> onCastSkill;

    protected List<DesIconInfo> desInfos = new List<DesIconInfo>();

    public bool IsInRullet { get; set; } = false;
    public Inventory Owner { get; set; }

    protected AnimHandler animHandler = null;

    private GameObject go;
    private string content;

    protected BattleHandler bh;
    protected override void Awake()
    {
        base.Awake();

        PieceType = PieceType.SKILL;

        for (int i = 0; i < 3; i++)
        {
            desInfos.Add(new DesIconInfo());
        }

    }

    protected virtual void Start()
    {
        bh = GameManager.Instance.battleHandler;
        animHandler = GameManager.Instance.animHandler;
    }

    public override void OnRullet()
    {
        base.OnRullet();
        bh = GameManager.Instance.battleHandler;
        animHandler = GameManager.Instance.animHandler;
    }
    public virtual int GetDamageCalc() //Player 전용임
    {
        int attack = Owner.GetComponent<LivingEntity>().AttackPower + value;
        return attack;
    }

    public virtual int GetDamageCalc(int addValue) //Player 전용임
    {
        int attack = Owner.GetComponent<LivingEntity>().AttackPower + addValue;
        return attack;
    }

    public virtual PieceInfo ChoiceSkill()
    {
        ResetDesInfo();
        ResetIconInfo();

        return new PieceInfo("None", "None");
    }

    private void ResetIconInfo()
    {
        usedIcons.Clear();
    }

    public override void Cast(LivingEntity targetTrm, Action onCastEnd = null) { }

    public virtual string GetPieceDes()
    {
        string des = PieceDes;
        des.Replace("{OwnerDmg}", Owner.GetComponent<LivingEntity>().AttackPower.ToString());
        return PieceDes;
    }

    public virtual List<DesIconInfo> GetDesIconInfo()
    {
        ResetDesInfo();
        return desInfos;
    }

    public void ResetDesInfo()
    {
        for (int i = 0; i < desInfos.Count; i++)
        {
            desInfos[i].iconType = DesIconType.None;
        }
    }

    public void AddAttackPower(int value)
    {
        Owner.GetComponent<LivingEntity>().AddAttackPower(value);
    }

    public virtual bool CheckCC(CCType type)
    {
        CrowdControl cc = Owner.GetComponent<CrowdControl>();
        return cc.ccDic[type] > 0;
    }

    protected SkillPiece SetIndicator(GameObject go, string content) //머리위에 텍스트 뛰우는 함수
    {
        this.go = go;
        this.content = content;
        return this;
    }

    public void OnEndAction(Action action) //스킬이 끝났을때
    {
        EnemyIndicator enemyIndicator = go.GetComponent<EnemyIndicator>();
        if (enemyIndicator != null)
        {
            enemyIndicator.ShowText(content, action);
        }
    }

}

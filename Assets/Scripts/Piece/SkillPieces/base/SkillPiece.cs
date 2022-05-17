using System;
using System.Collections.Generic;
using UnityEngine;

public class SkillPiece : RulletPiece
{
    [Header("��ų ����")]
    public bool isPlayerSkill = true;                 // �÷��̾� ��ų�ΰ�?
    public bool isTargeting = true;                   // Ÿ���ý�ų�ΰ�?
    public bool isRandomSkill = false;                // ���� ��ų�ΰ�??
    public bool isDisposable = false;                 // 1ȸ���ΰ�
    public SkillRange skillRange = SkillRange.Single; // ��ų�� ���� ����

    public PieceInfo[] pieceInfo;
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

    public virtual PieceInfo ChoiceSkill()
    {
        return new PieceInfo("None", "None");
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
        for (int i = 0; i < desInfos.Count; i++)
        {
            desInfos[i].iconType = DesIconType.None;
        }
        return desInfos;
    }

    public virtual bool CheckSilence() // ħ�� �����ΰ�?
    {
        CrowdControl cc = Owner.GetComponent<CrowdControl>();
        return cc.ccDic[CCType.Silence] > 0;
    }

    protected SkillPiece SetIndicator(GameObject go, string content) //�Ӹ����� �ؽ�Ʈ �ٿ�� �Լ�
    {
        this.go = go;
        this.content = content;
        return this;
    }

    public void OnEndAction(Action action) //��ų�� ��������
    {
        EnemyIndicator enemyIndicator = go.GetComponent<EnemyIndicator>();
        if (enemyIndicator != null)
        {
            enemyIndicator.ShowText(content, action);
        }
    }

}

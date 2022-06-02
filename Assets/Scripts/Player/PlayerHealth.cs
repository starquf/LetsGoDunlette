using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    private int maxPieceCount;
    private int playerLevel;
    private int maxExp;
    private int currentExp;

    public Color damageBGColor;
    public Image damageBGEffect;

    public TextMeshProUGUI topPanelHPText;

    public int MaxPieceCount { get => maxPieceCount; set => maxPieceCount = value; }
    public int PlayerLevel { get => playerLevel; set => playerLevel = value; }
    public int MaxExp { get => maxExp; set => maxExp = value; }
    public int CurrentExp { get => currentExp; set => currentExp = value; }

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.battleHandler.player = this;
        cc.isPlayer = true;

        PlayerLevel = 1;
        CurrentExp = 0;
        MaxExp = 100;
        MaxPieceCount = 13;
    }

    public override void Init()
    {
        base.Init();
        PlayerLevel = 1;
        CurrentExp = 0;
        MaxExp = 100;
        MaxPieceCount = 13; 
    }

    public override void SetHPBar()
    {
        topPanelHPText.text = IsDie ? $"»ç¸Á" : $"{hp}/{maxHp}";
        base.SetHPBar();
    }

    public override void GetDamage(int damage, bool isCritical = false)
    {
        base.GetDamage(damage);

        damageBGEffect.color = damageBGColor;
        damageBGEffect.DOFade(0f, 0.55f);
    }

    public override void Heal(int value)
    {
        base.Heal(value);

        if (!GameManager.Instance.curEncounter.Equals(mapNode.RandomEncounter) && !GameManager.Instance.curEncounter.Equals(mapNode.REST))
        {
            GameManager.Instance.animHandler.GetAnim(AnimName.PlayerHeal).SetPosition(bh.mainRullet.transform.position)
                .SetScale(2.5f)
                .Play();

        }
        bool hasEffect = GameManager.Instance.buffParticleHandler.otherParticleSetterDic.TryGetValue("Heal", out BuffParticleSetter bPS);
        if (hasEffect)
        {
            bPS.Play(0.55f);
        }
    }

    public void AddExp(int value)
    {
        //°æÇèÄ¡ Áõ°¡ ÆË¾÷
        int prevLevel = PlayerLevel;
        int prevExp = CurrentExp;

        for (int i = 0; i < value; i++)
        {
            CurrentExp++;
            if (CheckLevelUP())
            {
                LevelUP();
            }
        }

        GameManager.Instance.uILevelUPPopUp.PopUp(prevLevel, prevExp, PlayerLevel, CurrentExp, MaxExp);
        GameManager.Instance.battleHandler.playerInfoHandler.Synchronization();
    }

    private bool CheckLevelUP()
    {
        return CurrentExp == MaxExp;
    }

    public void LevelUP()
    {
        PlayerLevel++;
        CurrentExp = 0;
        //º¸»ó ÆË¾÷ ¶ç¿ö¾ßÇÔ
    }

    public void UpgradeMaxPieceCount(int value)
    {
        MaxPieceCount += value;
    }
}

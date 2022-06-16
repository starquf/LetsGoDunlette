using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public int atkLevel;
    public int hpLevel;
    public int maxPieceLevel;

    public Color damageBGColor;
    public Image damageBGEffect;

    public TextMeshProUGUI topPanelHPText;
    public int maxPlayerLevel;

    public int PlayerLevel { get; set; }
    public int MaxPieceCount { get; set; }
    public int MaxExp { get; set; }
    public int CurrentExp { get; set; }

    protected override void Awake()
    {
        base.Awake();
        GameManager.Instance.battleHandler.player = this;
        cc.isPlayer = true;

        PlayerLevel = 1;
        CurrentExp = 0;
        MaxExp = 100;
        MaxPieceCount = 10;
        atkLevel = 0;
        hpLevel = 0;
        maxPieceLevel = 0;
    }

    public override void Init()
    {
        base.Init();

        PlayerLevel = 1;
        CurrentExp = 0;
        MaxExp = 100;
        MaxPieceCount = 10;
        atkLevel = 0;
        hpLevel = 0;
        maxPieceLevel = 0;
    }

    public override void SetHPBar()
    {
        topPanelHPText.text = IsDie ? $"»ç¸Á" : $"{hp}/{maxHp}";
        base.SetHPBar();
    }

    public override void GetDamage(int damage, bool isCritical = false)
    {
        base.GetDamage(damage);

        if (isDie && GameManager.Instance.curEncounter.Equals(mapNode.RandomEncounter))
        {
            GameManager.Instance.ResetGame();
            Init();
            //GameManager.Instance.battleHandler.GetComponent<BattleRewardHandler>().ResetRullet(() =>
            //{
            //});
        }

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

    public bool IsMaxLevel()
    {
        if (PlayerLevel >= maxPlayerLevel)
        {
            PlayerLevel = maxPlayerLevel;
            CurrentExp = 0;
            return true;
        }
        return false;
    }

    public void AddExp(int value, List<ExpLog> expLogs = null)
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

        IsMaxLevel();

        GameManager.Instance.uILevelUPPopUp.PopUp(prevLevel, prevExp, PlayerLevel, CurrentExp, MaxExp, expLogs);
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

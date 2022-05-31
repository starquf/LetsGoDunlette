using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : LivingEntity
{
    public int playerLevel;

    public int maxExp;

    private int currentExp;

    public Color damageBGColor;
    public Image damageBGEffect;

    public TextMeshProUGUI topPanelHPText;

    protected override void Awake()
    {
        base.Awake();

        cc.isPlayer = true;
    }

    public override void Init()
    {
        base.Init();
        playerLevel = 1;
        maxExp = 100;
        currentExp = 0;
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

        for (int i = 0; i < value; i++)
        {
            currentExp++;
            if (CheckLevelUP())
            {
                LevelUP();
            }
        }
    }

    private bool CheckLevelUP()
    {
        return currentExp == maxExp;
    }

    public void LevelUP()
    {
        playerLevel++;
        currentExp = 0;

        //º¸»ó ÆË¾÷ ¶ç¿ö¾ßÇÔ
    }
}

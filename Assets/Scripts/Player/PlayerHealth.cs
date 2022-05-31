using DG.Tweening;
using System.Collections;
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
        BuffParticleSetter bPS = null;
        bool hasEffect = GameManager.Instance.buffParticleHandler.otherParticleSetterDic.TryGetValue("Heal", out bPS);
        if (hasEffect)
        {
            bPS.Play(0.55f);
        }
    }

    public void AddExp(int value)
    {
        for (int i = 0; i < value; i++)
        {
            currentExp++;
            if(CheckLevelUP())
            {
                LevelUP();
            }
        }
    }

    private bool CheckLevelUP()
    {
        if(currentExp == maxExp)
        {
            return true;
        }

        return false;
    }

    public void LevelUP() 
    {
        playerLevel++;
        currentExp = 0;
    }
}

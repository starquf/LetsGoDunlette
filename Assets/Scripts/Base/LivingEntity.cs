using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    // 이거 나중에 클래스로 뺴줘요
    public GameObject hPCvs;

    [HideInInspector]
    public Image hpBar;
    private Image hpBarAfterImageBar;
    private Image hpShieldBar;

    private Text hpText;

    private Transform damageTrans;

    private Image damageImg;
    private Color damageColor;
    private Tween damageTween;

    public int maxHp;
    public int curMaxHp => maxHp + shieldHp;
    public int curHp => hp + shieldHp;

    [SerializeField] protected int hp;
    [SerializeField] protected int shieldHp = 0;

    protected bool isDie = false;
    public bool IsDie => isDie;

    protected BattleHandler bh;

    public PatternType weaknessType;

    [HideInInspector]
    public CrowdControl cc;

    [HideInInspector]
    public Vector3 originSize;
    protected virtual void Awake()
    {
        cc = GetComponent<CrowdControl>();

        Transform bar = hPCvs.transform.Find("HPBar").transform;
        hpBar = bar.Find("HPBar").GetComponent<Image>();
        hpBarAfterImageBar = bar.Find("HPAfterImageBar").GetComponent<Image>();
        hpShieldBar = bar.Find("HPShieldBar").GetComponent<Image>();
        hpText = bar.Find("hpText").GetComponent<Text>();
        damageTrans = hpBar.transform.Find("DamageEffect").transform;

        damageImg = damageTrans.GetComponent<Image>();
        damageColor = damageImg.color;
        damageColor = new Color(damageColor.r, damageColor.g, damageColor.b, 1f);

        originSize = transform.localScale;
    }

    protected virtual void Start()
    {
        hp = maxHp;
        shieldHp = 0;

        SetHPBar();

        bh = GameManager.Instance.battleHandler;
    }

    public virtual void SetScale(float percent)
    {
        transform.DOScale(originSize * percent, 0.3f);
    }

    public virtual void GetDamage(int damage)
    {
        if (isDie)
        {
            return;
        }

        if (cc.ccDic[CCType.Invincibility] > 0)
        {
            return;
        }

        // 계약 상태라면
        if (cc.buffDic[BuffType.Contract] > 0)
        {
            damage = cc.buffDic[BuffType.Contract];
            cc.RemoveBuff(BuffType.Contract);
        }

        if (shieldHp > 0)
        {
            int left = shieldHp - damage;
            if (left < 0) // -1라면
            {
                hp += left;
                shieldHp = 0;
                cc.RemoveBuff(BuffType.Shield);
            }
            else
            {
                shieldHp = left;
                cc.DecreaseBuff(BuffType.Shield, damage);
            }
        }
        else
        {
            hp -= damage;
        }

        SetHPBar();

        if (hp <= 0)
        {
            hp = 0;
            isDie = true;

            Die();
        }

        Anim_TextUp damageTextEffect = PoolManager.GetItem<Anim_TextUp>();
        damageTextEffect.SetType(TextUpAnimType.Damage);
        damageTextEffect.transform.position = transform.position;
        damageTextEffect.Play(damage.ToString());

        SetDamageEffect();
    }

    public virtual void SetHp(int hp)
    {
        if (isDie)
        {
            return;
        }

        RemoveShield();

        this.hp = hp;

        if (hp <= 0)
        {
            hp = 0;
            isDie = true;

            Die();
        }

        SetHPBar();
    }

    public virtual void GetDamage(int damage, PatternType damageType)
    {
        if (weaknessType.Equals(damageType) && !weaknessType.Equals(PatternType.None))
        {
            GetDamage((int)(damage * 1.5f));
        }
        else
        {
            GetDamage(damage);
        }
    }

    public virtual void GetDamage(int damage, SkillPiece skillPiece,Inventory owner) // 적이 사용 전용
    {
        Vector3 size = owner.transform.localScale;
        SpriteRenderer sr = owner.GetComponent<SpriteRenderer>();

        DOTween.Sequence()
            .AppendCallback(() =>
            {
                sr.sortingLayerID = SortingLayer.NameToID("Effect");
                sr.sortingOrder = -1;
            })
            .Append(owner.transform.DOScale(size * 2f, 0.15f))
            .Insert(0.1f, owner.transform.DOShakePosition(0.3f, 0.25f, 50, 90f))
            //.AppendInterval(0.3f)
            .Append(owner.transform.DOScale(size, 0.5f))
            .AppendCallback(() =>
            {
                sr.sortingLayerID = SortingLayer.NameToID("Default");
                sr.sortingOrder = 1;
            });

        if(skillPiece.currentType == GameManager.Instance.battleFieldHandler.FieldType)
        {
            damage += 20;
        }

        GetDamage(damage);
    }

    public virtual void GetDamageIgnoreShild(int damage) //쉴드 무시하고 받는 데미지
    {
        if (isDie)
        {
            return;
        }

        // 계약 상태라면
        if (cc.buffDic[BuffType.Contract] > 0)
        {
            damage = cc.buffDic[BuffType.Contract];
            cc.RemoveBuff(BuffType.Contract);
        }

        hp -= damage;

        if (hp <= 0)
        {
            hp = 0;
            isDie = true;

            Die();
        }

        Anim_TextUp damageTextEffect = PoolManager.GetItem<Anim_TextUp>();
        damageTextEffect.SetType(TextUpAnimType.Damage);
        damageTextEffect.transform.position = transform.position;
        damageTextEffect.Play(damage.ToString());

        SetDamageEffect();
        SetHPBar();
    }

    public virtual void Heal(int value) //value 만큼 회복합니다.
    {
        hp += value;
        hp = Mathf.Clamp(hp, 0, maxHp);

        SetHPBar();
    }

    private void SetDamageEffect()
    {
        DOTween.To(() => damageImg.fillAmount, x => damageImg.fillAmount = x, ((float)hp + shieldHp) / (hp + shieldHp > maxHp ? hp + shieldHp : maxHp), 0.33f);
        //damageTrans.DOScaleX(hp / (float)maxHp, 0.33f);

        damageImg.color = damageColor;

        damageTween.Kill();
        damageTween = damageImg.DOFade(0f, 0.3f);
    }

    public virtual void AddShield(int value)
    {
        cc.IncreaseBuff(BuffType.Shield, value);

        shieldHp += value;
        SetHPBar();
    }

    public virtual void RemoveShield()
    {
        cc.RemoveBuff(BuffType.Shield);

        shieldHp = 0;
        SetHPBar();
    }

    public virtual void Init()
    {
        isDie = false;
        hp = maxHp;

        cc.ResetAllCC();
        cc.RemoveBuff(BuffType.Shield);

        shieldHp = 0;

        SetHPBar();
    }

    protected virtual void SetHPBar()
    {

        hpText.text = $"{hp}/{maxHp}";

        if(shieldHp > 0)
        {
            hpText.text = $"{hp}+<color=aqua>{shieldHp}</color>/{maxHp}";
        }

        if (hp <= 0)
        {
            hpText.text = $"사망";
        }

        if (hp == maxHp)
        {
            DOTween.To(() => hpBar.fillAmount, x => hpBar.fillAmount = x, (float)hp / curMaxHp, 0.33f);
            DOTween.To(() => hpShieldBar.fillAmount, x => hpShieldBar.fillAmount = x, ((float)hp + shieldHp) / curMaxHp, 0.33f).OnComplete(() =>
            {
                DOTween.To(() => hpBarAfterImageBar.fillAmount, x => hpBarAfterImageBar.fillAmount = x, (float)hp / curMaxHp, 0.33f);
            });
        }
        else if (hp + shieldHp > maxHp)
        {
            float max = hp + shieldHp;
            //hpBar.DOScaleX((float)hp / max, 0.33f);
            //hpShieldBar.DOScaleX((float)shieldHp / max, 0.33f);

            DOTween.To(() => hpBar.fillAmount, x => hpBar.fillAmount = x, (float)hp / max, 0.33f);
            DOTween.To(() => hpShieldBar.fillAmount, x => hpShieldBar.fillAmount = x, ((float)hp + shieldHp) / max, 0.33f).OnComplete(() =>
            {
                DOTween.To(() => hpBarAfterImageBar.fillAmount, x => hpBarAfterImageBar.fillAmount = x, (float)hp / max, 0.33f);
            });
        }
        else
        {
            //hpBar.DOScaleX((float)hp / maxHp, 0.33f);
            //hpShieldBar.DOScaleX((float)shieldHp / maxHp, 0.33f);
            DOTween.To(() => hpBar.fillAmount, x => hpBar.fillAmount = x, (float)hp / maxHp, 0.33f);
            DOTween.To(() => hpShieldBar.fillAmount, x => hpShieldBar.fillAmount = x, ((float)hp + shieldHp) / maxHp, 0.33f).OnComplete(() =>
            {
                DOTween.To(() => hpBarAfterImageBar.fillAmount, x => hpBarAfterImageBar.fillAmount = x, (float)hp / maxHp, 0.33f);
            });
        }


    }

    public void ChangeShieldToHealth()
    {
        if(shieldHp > 0)
        {
            int previousShield = shieldHp;
            shieldHp = 0;

            cc.RemoveBuff(BuffType.Shield);

            Heal(previousShield);
        }

        SetHPBar();
    }

    public bool HasShield()
    {
        if (shieldHp > 0)
        {
            return true;
        }
        return false;
    }

    public void RemoveAllShield()
    {
        shieldHp = 0;
        SetHPBar();
    }

    public int GetShieldHp()
    {
        return shieldHp;
    }

    public float GetHpRatio()
    {
        return ((float)hp / maxHp) * 100;
    }
    protected abstract void Die();
}

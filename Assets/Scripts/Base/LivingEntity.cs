using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    public int maxHp;
    public int curMaxHp => maxHp + shieldHp; //MaxHp �� ShieldHp ���Ѱ�
    public int curHp => hp + shieldHp; // ���� ü�¿� ���� ���Ѱ�

    [SerializeField] protected int hp;
    [SerializeField] protected int shieldHp = 0;
    [SerializeField] protected int attackPower;
    public int AttackPower
    {
        get
        {
            return attackPower;
        }
    }

    // �̰� ���߿� Ŭ������ �����
    public GameObject hPCvs;
    [HideInInspector] public Image hpBar;

    private Image hpBarAfterImageBar;
    private Image hpShieldBar;
    private Text hpText;
    private Transform damageTrans;
    private Image damageImg;
    private Color damageColor;
    private Tween damageTween;
    public bool IsDie => isDie;
    protected bool isDie = false;
    protected BattleHandler bh;
    public ElementalType weaknessType;

    [HideInInspector] public CrowdControl cc;

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
    }

    protected virtual void Start()
    {
        bh = GameManager.Instance.battleHandler;

        hp = maxHp;
        shieldHp = 0;

        SetHPBar();
    }

    public virtual void Init()
    {
        isDie = false;
        hp = maxHp;
        cc.ResetAllCC();
        RemoveShield();

        SetHPBar();
    }

    public virtual void SetHp(int hp)
    {
        if (IsDie)
        {
            return;
        }

        this.hp = hp;

        RemoveShield();
        SetHPBar();

        if (hp <= 0)
        {
            Die();
        }
    }

    public virtual void GetDamage(int damage, bool isCritical = false)
    {
        if (cc.ccDic[CCType.Invincibility] > 0 || isDie) //�̹� �׾��ų� ���� ���¶��
        {
            return;
        }

        if (shieldHp > 0) //���尡 �ִٸ�
        {
            int leftDamage = shieldHp - damage; //���带 �� ���� ���� �����
            if (leftDamage < 0)
            {
                hp += leftDamage;
                RemoveShield();
            }
            else
            {
                shieldHp = leftDamage;
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
            Die();
        }

        ShowDamageText(damage, isCritical);
        SetDamageEffect();
    }

    public void ShowDamageText(int damage,bool isCritical)
    {
        Anim_TextUp text = GameManager.Instance.animHandler.GetTextAnim();
        text.SetType(TextUpAnimType.Volcano);
        text.SetPosition(transform.position);
        text.SetScale(0.9f + (damage / 200f));

        if (isCritical)
        {
            Color32 criColor = new Color32(255, 220, 0, 255);

            text.SetTextColor(criColor);

            GameManager.Instance.animHandler.GetTextAnim()
            .SetType(TextUpAnimType.Fixed)
            .SetPosition(transform.position)
            .SetScale(0.8f)
            .SetTextColor(criColor)
            .Play("����!");
        }

        text.Play(damage.ToString());
    }

    public virtual void GetDamage(int damage, ElementalType damageType)
    {
        BattleFieldHandler fieldHandler = GameManager.Instance.battleHandler.fieldHandler;

        float damageBuff = 1f;

        if (fieldHandler.FieldType != ElementalType.None && damageType.Equals(fieldHandler.FieldType))
        {
            damageBuff += 0.5f;
        }

        if (weaknessType.Equals(damageType) && !weaknessType.Equals(ElementalType.None))
        {
            damageBuff += 0.5f;
            GetDamage((int)(damage * damageBuff), true);
        }
        else
        {
            GetDamage((int)(damage * damageBuff));
        }
    }

    public virtual void GetDamage(int damage, SkillPiece skillPiece, Inventory Owner) // ���� ��� ����
    {
        Vector3 size = Owner.transform.localScale;
        SpriteRenderer sr = Owner.GetComponent<SpriteRenderer>();

        DOTween.Sequence()
            .AppendCallback(() =>
            {
                sr.sortingLayerID = SortingLayer.NameToID("Effect");
                sr.sortingOrder = -1;
            })
            .Append(Owner.transform.DOScale(size * 2f, 0.15f))
            .Insert(0.1f, Owner.transform.DOShakePosition(0.3f, 0.25f, 50, 90f))
            //.AppendInterval(0.3f)
            .Append(Owner.transform.DOScale(size, 0.5f))
            .AppendCallback(() =>
            {
                sr.sortingLayerID = SortingLayer.NameToID("Default");
                sr.sortingOrder = 1;
            });

        if(skillPiece.currentType == GameManager.Instance.battleHandler.fieldHandler.FieldType)
        {
            damage += 20;
        }

        GetDamage(damage);
    }

    public virtual void GetDamageIgnoreShild(int damage) //���� �����ϰ� �޴� ������
    {
        if (IsDie)
        {
            return;
        }

        hp -= damage;

        if (hp <= 0)
        {
            Die();
        }

        GameManager.Instance.animHandler.GetTextAnim()
        .SetType(TextUpAnimType.Volcano)
        .SetPosition(transform.position)
        .Play(damage.ToString());

        SetDamageEffect();
        SetHPBar();
    }

    public virtual void Heal(int value) //value ��ŭ ȸ���մϴ�.
    {
        hp += value;
        hp = Mathf.Clamp(hp, 0, maxHp);

        SetHPBar();
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
    public void ChangeShieldToHealth()
    {
        if (shieldHp > 0)
        {
            int previousShield = shieldHp;

            RemoveShield();
            Heal(previousShield);
            SetHPBar();
        }
    }

    private void SetDamageEffect()
    {
        DOTween.To(() => damageImg.fillAmount, x => damageImg.fillAmount = x, ((float)hp + shieldHp) / (hp + shieldHp > maxHp ? hp + shieldHp : maxHp), 0.33f);
        damageImg.color = damageColor;
        damageTween.Kill();
        damageTween = damageImg.DOFade(0f, 0.3f);
    }

    private void SetHPBar()
    {
        if (IsDie)
        {
            hpText.text = $"���";
            return;
        }

        hpText.text = $"{hp}/{maxHp}";
        if(shieldHp > 0)
        {
            hpText.text = $"{hp}+<color=aqua>{shieldHp}</color>/{maxHp}";
        }

        if (hp == maxHp)
        {
            DOTween.To(() => hpBar.fillAmount, x => hpBar.fillAmount = x, (float)hp / curMaxHp, 0.33f);
            DOTween.To(() => hpShieldBar.fillAmount, x => hpShieldBar.fillAmount = x, ((float)hp + shieldHp) / curMaxHp, 0.33f).OnComplete(() =>
            {
                DOTween.To(() => hpBarAfterImageBar.fillAmount, x => hpBarAfterImageBar.fillAmount = x, (float)hp / curMaxHp, 0.33f);
            });
        }
        else if (curHp >= maxHp)
        {
            float max = curHp;

            DOTween.To(() => hpBar.fillAmount, x => hpBar.fillAmount = x, (float)hp / max, 0.33f);
            DOTween.To(() => hpShieldBar.fillAmount, x => hpShieldBar.fillAmount = x, 1, 0.33f).OnComplete(() =>
            {
                DOTween.To(() => hpBarAfterImageBar.fillAmount, x => hpBarAfterImageBar.fillAmount = x, (float)hp / max, 0.33f);
            });
        }
        else
        {
            DOTween.To(() => hpBar.fillAmount, x => hpBar.fillAmount = x, (float)hp / maxHp, 0.33f);
            DOTween.To(() => hpShieldBar.fillAmount, x => hpShieldBar.fillAmount = x, ((float)hp + shieldHp) / maxHp, 0.33f).OnComplete(() =>
            {
                DOTween.To(() => hpBarAfterImageBar.fillAmount, x => hpBarAfterImageBar.fillAmount = x, (float)hp / maxHp, 0.33f);
            });
        }
    }

    public bool HasShield()
    {
        if (shieldHp > 0)
        {
            return true;
        }
        return false;
    }

    public int GetShieldHp()
    {
        return shieldHp;
    }

    public float GetHpRatio()
    {
        return ((float)hp / maxHp) * 100;
    }
    protected virtual void Die()
    {
        hp = 0;
        isDie = true;
    }
}

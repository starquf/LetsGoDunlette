using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    [Header("기본 세팅")]

    public Vector2 atkRange;
    public Vector2 hpRange;

    public int maxHp;
    public int curMaxHp => maxHp + shieldHp; //MaxHp 에 ShieldHp 더한값
    public int curHp => hp + shieldHp; // 현재 체력에 쉴드 더한값

    [SerializeField] protected int hp;
    [SerializeField] protected int shieldHp = 0;
    [SerializeField] protected int attackPower;

    public int AttackPower
    {
        get
        {
            int atkPower = 0;

            if (cc != null)
            {
                atkPower = attackPower + addtionAttackPower + cc.buffDic[BuffType.Upgrade];

                if (cc.IsCC(CCType.Exhausted))
                {
                    atkPower = Mathf.RoundToInt(atkPower * 0.75f);
                }
            }
            else
            {
                atkPower = attackPower + addtionAttackPower;
            }

            return atkPower;
        }
    }

    [SerializeField] protected int addtionAttackPower;
    public int AddtionAttackPower
    {
        get => addtionAttackPower;
        set => addtionAttackPower = value;
    }

    // 이거 나중에 클래스로 뺴줘요
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

        if (hPCvs != null)
        {
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
    }

    protected virtual void Start()
    {
        bh = GameManager.Instance.battleHandler;

        maxHp = Random.Range((int)hpRange.x, (int)hpRange.y);
        attackPower = Random.Range((int)atkRange.x, (int)atkRange.y);

        hp = maxHp;
        shieldHp = 0;

        SetHPBar();
    }

    public virtual void Init()
    {
        isDie = false;

        maxHp = Random.Range((int)hpRange.x, (int)hpRange.y);
        attackPower = Random.Range((int)atkRange.x, (int)atkRange.y);

        hp = maxHp;
        addtionAttackPower = 0;

        cc.ResetAllCC();
        cc.ResetAllBuff();

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

    public virtual void UpgradeHP(int value) //레벨업 보상에 쓸예정
    {
        maxHp += value;
        hp += value;
        SetHPBar();
    }

    public virtual void UpgradeAttackPower(int value) //레벨업 보상에 쓸예정
    {
        attackPower += value;
    }

    public virtual void GetDamage(int damage, bool isCritical = false)
    {
        if (cc.ccDic[CCType.Invincibility] > 0 || isDie) //이미 죽었거나 무적 상태라면
        {
            return;
        }

        if (shieldHp > 0) //쉴드가 있다면
        {
            int leftDamage = shieldHp - damage; //쉴드를 다 쓰고 남은 대미지
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

        if (hp <= 0)
        {
            Die();
        }

        SetHPBar();

        ShowDamageText(damage, isCritical);
        SetDamageEffect();
    }

    public void ShowDamageText(int damage, bool isCritical)
    {
        Anim_TextUp text = GameManager.Instance.animHandler.GetTextAnim();
        text.SetType(TextUpAnimType.Volcano);
        text.SetPosition(transform.position);
        text.SetScale(0.9f + (damage / 20f));

        if (isCritical)
        {
            Color32 criColor = new Color32(255, 220, 0, 255);

            text.SetTextColor(criColor);

            GameManager.Instance.animHandler.GetTextAnim()
            .SetType(TextUpAnimType.Fixed)
            .SetPosition(transform.position)
            .SetScale(0.8f)
            .SetTextColor(criColor)
            .Play("약점!");
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

    public virtual void GetDamage(int damage, SkillPiece skillPiece, Inventory Owner) // 적이 사용 전용
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

        if (skillPiece.currentType == GameManager.Instance.battleHandler.fieldHandler.FieldType)
        {
            damage += 20;
        }

        GetDamage(damage);
    }

    public virtual void GetDamageIgnoreShild(int damage) //쉴드 무시하고 받는 데미지
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

    public virtual void AddAttackPower(int value)
    {
        addtionAttackPower += value;
    }

    public virtual void Heal(int value) //value 만큼 회복합니다.
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

    public virtual void RemoveShieldRatio(float ratio)
    {
       int removeRatio = (int)(shieldHp * (ratio / 100));
        cc.DecreaseBuff(BuffType.Shield, removeRatio);

        shieldHp -= removeRatio;
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

    public virtual void SetHPBar()
    {
        hpText.text = IsDie ? $"사망" : $"{hp}/{maxHp}";

        if (shieldHp > 0)
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

            DOTween.To(() => hpBar.fillAmount, x => hpBar.fillAmount = x, hp / max, 0.33f);
            DOTween.To(() => hpShieldBar.fillAmount, x => hpShieldBar.fillAmount = x, 1, 0.33f).OnComplete(() =>
            {
                DOTween.To(() => hpBarAfterImageBar.fillAmount, x => hpBarAfterImageBar.fillAmount = x, hp / max, 0.33f);
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
        return shieldHp > 0;
    }

    public int GetShieldHp()
    {
        return shieldHp;
    }

    public float GetHpRatio()
    {
        return (float)hp / maxHp * 100;
    }
    protected virtual void Die()
    {
        hp = 0;
        isDie = true;
    }
}

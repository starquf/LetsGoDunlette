using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    // 이거 나중에 클래스로 뺴줘요
    public Transform hpBar;
    public Transform hpShieldBar;
    public Text hpText;

    public Transform damageTrans;

    private Image damageImg;
    private Color damageColor;
    private Tween damageTween;

    public int maxHp;
    private int curMaxHp => maxHp + shieldHp;

    [SerializeField] protected int hp;
    [SerializeField] protected int shieldHp = 0;

    protected bool isDie = false;
    public bool IsDie => isDie;

    protected BattleHandler bh;

    public PatternType weaknessType;

    [HideInInspector]
    public CrowdControl cc;

    private void Awake()
    {
        cc = GetComponent<CrowdControl>();
    }

    protected virtual void Start()
    {
        hp = maxHp;
        shieldHp = 0;
        SetHpText();

        bh = GameManager.Instance.battleHandler;
        damageImg = damageTrans.GetComponent<Image>();

        damageColor = damageImg.color;
        damageColor = new Color(damageColor.r, damageColor.g, damageColor.b, 1f);
    }

    public virtual void GetDamage(int damage)
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

        SetHpText();

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

    public virtual void GetDamage(int damage, PatternType damageType)
    {
        if (weaknessType.Equals(damageType))
        {
            GetDamage((int)(damage * 1.5f));
        }
        else
        {
            GetDamage(damage);
        }
    }

    public virtual void GetDamage(int damage, GameObject owner) // 적 전용임
    {
        DOTween.Sequence()
            .Append(owner.transform.DOScale(Vector3.one * 3f, 0.15f))
            .Insert(0.1f, owner.transform.DOShakePosition(0.3f, 0.25f, 50, 90f))
            //.AppendInterval(0.3f)
            .Append(owner.transform.DOScale(Vector3.one * 2f, 0.5f));

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
        SetHpText();
    }

    public virtual void Heal(int value) //value 만큼 회복합니다.
    {
        hp += value;
        hp = Mathf.Clamp(hp, 0, maxHp);

        SetHpText();
    }

    private void SetDamageEffect()
    {
        damageTrans.DOScaleX(hp / (float)maxHp, 0.33f);

        damageImg.color = damageColor;

        damageTween.Kill();
        damageTween = damageImg.DOFade(0f, 0.3f);
    }

    public virtual void AddShield(int value)
    {
        cc.IncreaseBuff(BuffType.Shield, value);

        shieldHp += value;
        SetHpText();
    }

    public virtual void Revive()
    {
        isDie = false;
        hp = maxHp;

        SetHpText();

        hpBar.DOScaleX(1f, 0.33f);
    }

    protected virtual void SetHpText()
    {
        hpText.text = $"{hp}/{maxHp}";
        if (hp == maxHp)
        {
            hpBar.DOScaleX((float)hp / curMaxHp, 0.33f);
            hpShieldBar.DOScaleX((float)shieldHp / curMaxHp, 0.33f);
        }
        else if (hp + shieldHp > maxHp)
        {
            float max = hp + shieldHp;
            hpBar.DOScaleX((float)hp / max, 0.33f);
            hpShieldBar.DOScaleX((float)shieldHp / max, 0.33f);
        }
        else
        {
            hpBar.DOScaleX((float)hp / maxHp, 0.33f);
            hpShieldBar.DOScaleX((float)shieldHp / maxHp, 0.33f);
        }
    }

    protected abstract void Die();
}

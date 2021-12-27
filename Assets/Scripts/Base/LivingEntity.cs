using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    // 이거 나중에 클래스로 뺴줘요
    public Transform hpBar;
    public Text hpText;

    public Transform damageTrans;

    private Image damageImg;
    private Color damageColor;
    private Tween damageTween;

    public int maxHp;
    [SerializeField] protected int hp;

    protected bool isDie = false;
    public bool IsDie => isDie;

    private BattleHandler bh;

    protected virtual void Start()
    {
        hp = maxHp;
        SetHpText();

        bh = GameManager.Instance.battleHandler;
        damageImg = damageTrans.GetComponent<Image>();

        damageColor = damageImg.color;
        damageColor = new Color(damageColor.r, damageColor.g, damageColor.b, 1f);
    }

    public virtual void GetDamage(int damage)
    {
        if (isDie) return;

        if (bh.IsContract)
        {
            damage = bh.ContractDmg;
            bh.IsContract = false;
        }

        hp -= damage;

        SetHpText();

        if (hp <= 0)
        {
            hp = 0;
            isDie = true;

            Die();
        }

        hpBar.DOScaleX(hp / (float)maxHp, 0.33f);
        SetDamageEffect();
    }

    private void SetDamageEffect()
    {
        damageTrans.DOScaleX(hp / (float)maxHp, 0.33f);

        damageImg.color = damageColor;

        damageTween.Kill();
        damageTween = damageImg.DOFade(0f, 0.3f);
    }

    public virtual void Revive()
    {
        isDie = false;
        hp = maxHp;

        hpBar.DOScaleX(1f, 0.33f);
    }

    protected virtual void SetHpText()
    {
        hpText.text = $"{hp}/{maxHp}";
    }

    protected abstract void Die();
}

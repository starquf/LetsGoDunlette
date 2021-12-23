using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    public Transform hpBar;

    public int maxHp;
    [SerializeField] protected int hp;

    protected bool isDie = false;
    public bool IsDie => isDie;

    private BattleHandler bh;

    protected virtual void Start()
    {
        hp = maxHp;

        bh = GameManager.Instance.battleHandler;
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

        if (hp <= 0)
        {
            hp = 0;
            isDie = true;

            Die();
        }

        hpBar.DOScaleX(hp / (float)maxHp, 0.33f);
    }

    public virtual void Revive()
    {
        isDie = false;
        hp = maxHp;

        hpBar.DOScaleX(1f, 0.33f);
    }

    protected abstract void Die();
}

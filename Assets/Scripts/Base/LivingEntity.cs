using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    // �ִ� HP (�ν����� �󿡼� ���氡��)
    public int maxHp;
    // ���� HP
    public int CurrHp { get; protected set; }

    // �׾��� �� ȣ��Ǵ� �̺�Ʈ
    public event Action OnDeath;

    protected virtual void Start()
    {
        CurrHp = maxHp;
    }

    public void GiveDamage(int damage)
    {
        CurrHp -= damage;

        if (CurrHp <= 0f)
        {
            OnDeath?.Invoke();
        }
    }
}

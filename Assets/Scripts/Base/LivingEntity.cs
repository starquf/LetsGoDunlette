using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LivingEntity : MonoBehaviour, IDamageable
{
    // 최대 HP (인스펙터 상에서 변경가능)
    public int maxHp;
    // 현재 HP
    public int CurrHp { get; protected set; }

    // 죽었을 때 호출되는 이벤트
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

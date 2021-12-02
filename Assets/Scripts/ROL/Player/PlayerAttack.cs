using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public void AttackSkill(List<RulletPiece> results, IDamageable target)
    {
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i] != null)
            {
                GiveDamage(results[i].Cast(), target);
            }
            else
            {
                NormalAttack(target);
            }
        }
    }

    private void NormalAttack(IDamageable target)
    {
        print("기본 공격!");
        target.GetDamage(5);
    }

    private void GiveDamage(int damage, IDamageable target)
    {
        target.GetDamage(damage);
    }
}

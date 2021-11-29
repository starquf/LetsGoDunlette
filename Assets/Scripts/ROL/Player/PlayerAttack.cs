using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public LivingEntity enemy;

    public void AttackSkill(List<RulletPiece> results)
    {
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i] != null)
            {
                GiveDamage(results[i].Cast());
            }
            else
            {
                NormalAttack();
            }
        }
    }

    private void NormalAttack()
    {
        print("기본 공격!");
        enemy.GetDamage(5);
    }

    private void GiveDamage(int damage)
    {
        enemy.GetDamage(damage);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public void Attack(IDamageable target)
    {
        target.GetDamage(10);
        print("Рћ АјАн!");
    }
}

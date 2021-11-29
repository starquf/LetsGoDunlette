using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public LivingEntity player;

    public void Attack()
    {
        player.GetDamage(10);
        print("Рћ АјАн!");
    }
}

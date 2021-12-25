using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    public CrowdControl cc;

    private void Awake()
    {
        cc = GetComponent<CrowdControl>();
    }

    protected override void Die()
    {
        print("³ª Á×À½!");
    }
}

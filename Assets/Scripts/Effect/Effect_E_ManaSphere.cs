using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_E_ManaSphere : EffectObj
{
    public Vector3 targetPos;

    private void Update()
    {
        Vector3 dir = targetPos - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        Quaternion quaternion = Quaternion.AngleAxis(angle, Vector3.forward);

        transform.rotation = quaternion;
    }
}

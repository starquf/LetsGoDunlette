using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_W_Bubble : SkillPiece
{
    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
       bh.battleUtil.StartCoroutine(Bubble(target, onCastEnd));
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(Owner.GetComponent<LivingEntity>().AttackPower * 0.4f);

        return attack;
    }

    private IEnumerator Bubble(LivingEntity target, Action onCastEnd = null)
    {
        LivingEntity t = Owner.GetComponent<LivingEntity>();

        int rand = Random.Range(5, 8);
        float time = 0.2f;
        for (int i = 0; i < rand; i++)
        {
            int a = i;

            animHandler.GetAnim(AnimName.W_Bubble)
            .SetPosition(t.transform.position + (Vector3)(Vector2.up * Random.Range(-0.3f, 0.3f) + Vector2.right * Random.Range(-0.3f, 0.3f)))
            .SetScale(0.5f)
            .Play(() => 
            {
                if (a == rand - 1)
                {
                    t.AddShield(Value);
                    animHandler.GetTextAnim()
                    .SetType(TextUpAnimType.Up)
                    .SetPosition(t.transform.position)
                    .Play("½¯µå!");

                    animHandler.GetAnim(AnimName.W_Shield)
                    .SetPosition(t.transform.position)
                    .SetScale(0.8f)
                    .Play(() =>
                    {
                        onCastEnd?.Invoke();
                    });
                }
            });

            yield return new WaitForSeconds(time / (float)rand);
        }
    }
}

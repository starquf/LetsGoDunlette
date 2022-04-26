using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_W_Bubble : SkillPiece
{
    BattleHandler bh;

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.battleHandler.battleUtil.StartCoroutine(Bubble(target, onCastEnd));
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();

        desInfos[0].SetInfo(DesIconType.Attack, GetDamageCalc().ToString());

        return desInfos;
    }

    private int GetDamageCalc()
    {
        int attack = (int)(owner.GetComponent<LivingEntity>().AttackPower * 0.4f);

        return attack;
    }

    private IEnumerator Bubble(LivingEntity target, Action onCastEnd = null)
    {
        //print($"��ų �ߵ�!! �̸� : {PieceName}");
        bh = GameManager.Instance.battleHandler;

        LivingEntity t = owner.GetComponent<LivingEntity>();

        int rand = Random.Range(5, 8);
        float time = 0.2f;
        for (int i = 0; i < rand; i++)
        {
            int a = i;
            Anim_W_Bubble effect = PoolManager.GetItem<Anim_W_Bubble>();
            effect.transform.position = t.transform.position + (Vector3)(Vector2.up*Random.Range(-0.3f,0.3f) + Vector2.right*Random.Range(-0.3f, 0.3f));
            effect.SetScale(0.5f);

            effect.Play(() => {

                if(a == rand-1)
                {
                    t.AddShield(Value);
                    Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                    textEffect.SetType(TextUpAnimType.Up);
                    textEffect.transform.position = t.transform.position;
                    textEffect.Play("����!");

                    Anim_Shield effect = PoolManager.GetItem<Anim_Shield>();
                    effect.transform.position = t.transform.position;
                    effect.SetScale(0.8f);
                    effect.Play(() =>
                    {
                        onCastEnd?.Invoke();
                    });
                }
            });
            yield return new WaitForSeconds(time / (float)rand);
        }
    }
}

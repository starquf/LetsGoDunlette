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
        StartCoroutine(Bubble(target, onCastEnd));

    }


    private IEnumerator Bubble(LivingEntity target, Action onCastEnd = null)
    {
        print($"스킬 발동!! 이름 : {PieceName}");
        bh = GameManager.Instance.battleHandler;
        target = bh.player;

        int rand = Random.Range(5, 8);
        float time = 0.2f;
        for (int i = 0; i < rand; i++)
        {
            int a = i;
            Anim_W_Bubble effect = PoolManager.GetItem<Anim_W_Bubble>();
            effect.transform.position = bh.playerImgTrans.position + (Vector3)(Vector2.up*Random.Range(-0.3f,0.3f) + Vector2.right*Random.Range(-0.3f, 0.3f));
            effect.SetScale(0.5f);

            effect.Play(() => {

                if(a == rand-1)
                {
                    target.AddShield(Value);

                    Anim_Shield effect = PoolManager.GetItem<Anim_Shield>();
                    effect.transform.position = bh.playerImgTrans.position;
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

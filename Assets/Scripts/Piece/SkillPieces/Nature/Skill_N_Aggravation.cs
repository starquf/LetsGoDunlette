using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_N_Aggravation : SkillPiece
{
    private readonly WaitForSeconds pSecWait = new WaitForSeconds(0.05f);

    protected override void Start()
    {
        base.Start();
        bh = GameManager.Instance.battleHandler;
    }

    public override List<DesIconInfo> GetDesIconInfo()
    {
        base.GetDesIconInfo();
        return desInfos;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null) //대상의 <sprite=12>를 2배로 늘린다.
    {
        bh.battleUtil.StartCoroutine(Aggravaion(target, onCastEnd));
    }
    private IEnumerator Aggravaion(LivingEntity target, Action onCastEnd = null)
    {
        print(2);
        animHandler.GetAnim(AnimName.BuffEffect04)
                .SetPosition(target.transform.position)
                .SetScale(1f)
                .SetColor(Color.red)
                .Play();
        for (int i = 0; i < 6; i++)
        {
            int idx = i;
            print(1);
            yield return pSecWait;
            print(3);
            animHandler.GetAnim(AnimName.M_Butt)
                    .SetPosition(target.transform.position + new Vector3(Random.Range(-0.8f, 0.8f), Random.Range(-0.8f, 0.8f), 0))
                    .SetScale(Random.Range(0.3f, 0.5f))
                    .SetColor(Color.red)
                    .Play(() =>
                    {
                        if(idx == 5)
                        {
                            onCastEnd?.Invoke();
                        }
                    });
        }
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
        target.cc.SetCC(CCType.Wound, target.cc.GetCCValue(CCType.Wound), true);
        yield return new WaitForSeconds(0.05f);
        onCastEnd?.Invoke();
    }
}

using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_N_Leech : SkillPiece
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

    public override void Cast(LivingEntity target, Action onCastEnd = null) //대상의 <sprite=12>만큼 <sprite=0>속성 추가 피해를 준다.
    {
        bh.battleUtil.StartCoroutine(Leech(target, onCastEnd));
    }

    private IEnumerator Leech(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);
        target.GetDamage(target.cc.GetCCValue(CCType.Wound), currentType);
        animHandler.GetAnim(AnimName.M_Bite)
                .SetPosition(target.transform.position)
                .SetScale(0.85f)
                .SetColor(Color.red)
                .Play();
        for (int i = 0; i < 6; i++)
        {
            int idx = i;
            yield return pSecWait;
            animHandler.GetAnim(AnimName.M_Butt)
                    .SetPosition(target.transform.position + new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0))
                    .SetScale(Random.Range(0.3f, 0.4f))
                    .SetColor(Color.red)
                    .Play(() =>
                    {
                        if (idx == 5)
                        {
                            onCastEnd?.Invoke();
                        }
                    });
        }
    }
}

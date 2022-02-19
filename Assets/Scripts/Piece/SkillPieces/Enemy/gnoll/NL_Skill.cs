using System;
using Random = UnityEngine.Random;

public class NL_Skill : SkillPiece
{
    private int addAdditionalDamage = 0;
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(1, 100) <= value)
        {
            NL_Poison_Dagger(target, onCastEnd);
        }
        else
        {
            NL_Mark(target, onCastEnd);
        }
    }

    private void NL_Poison_Dagger(LivingEntity target, Action onCastEnd = null) //상처를 부여해서 2턴 동안 10의 피해를 입힌다..
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
        effect.transform.position = owner.transform.position;

        target.cc.SetCC(CCType.Wound, 2);

        owner.GetComponent<EnemyIndicator>().ShowText("상처 부여");

        target.GetDamage(20 + addAdditionalDamage, owner.gameObject);


        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });



    }

    private void NL_Mark(LivingEntity target, Action onCastEnd = null) //놀의 모든 공격의 피해가 5 상승한다. 상처 제외
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
        effect.transform.position = owner.transform.position;

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });

        addAdditionalDamage += 5;

        for (int i = 0; i < owner.skills.Count; i++)
        {
            NL_Attack skill = owner.skills[i].GetComponent<NL_Attack>();
            if (skill != null)
            {
                skill.AddValue(5);

                // break; // 1개 라고 가정함
            }
        }
        owner.GetComponent<EnemyIndicator>().ShowText("강화");


    }


}

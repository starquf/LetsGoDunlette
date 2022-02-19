using System;
using Random = UnityEngine.Random;

public class MD_Skill : SkillPiece
{
    protected override void Awake()
    {
        base.Awake();
        isPlayerSkill = false;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        if (Random.Range(1, 100) <= value)
        {
            MD_Hallucinations(target, onCastEnd);
        }
        else
        {
            MD_Scream(target, onCastEnd);
        }
    }

    private void MD_Hallucinations(LivingEntity target, Action onCastEnd = null) //40% È®·ü·Î Ä§¹¬À» ºÎ¿©ÇÑ´Ù.
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
        effect.transform.position = owner.transform.position;

        target.GetDamage(15, owner.gameObject);

        effect.Play(() =>
        {
            if (Random.Range(1, 100) <= 40)
            {
                target.cc.SetCC(CCType.Silence, 2);

                owner.GetComponent<EnemyIndicator>().ShowText("Ä§¹¬ ºÎ¿©");

                Anim_M_Recover effect1 = PoolManager.GetItem<Anim_M_Recover>();
                effect1.transform.position = owner.transform.position;
                effect1.Play(() =>
                {
                    onCastEnd?.Invoke();
                });
            }
            else
            {
                onCastEnd?.Invoke();
            }
        });



    }

    private void MD_Scream(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Anim_M_Sword effect = PoolManager.GetItem<Anim_M_Sword>();
        effect.transform.position = owner.transform.position;

        effect.Play(() =>
        {
            onCastEnd?.Invoke();
        });

        target.GetDamage(35, owner.gameObject);
    }


}

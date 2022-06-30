using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS_MagicHat : PlayerSkill_Cooldown
{
    public List<GameObject> magicObjs = new List<GameObject>();
    public ParticleSystem magicParticle;

    private Inventory playerInven;

    protected override void Start()
    {
        base.Start();

        playerInven = bh.player.GetComponent<Inventory>();
    }

    public override void Cast(Action onEndSkill, Action onCancelSkill)
    {
        base.Cast(onEndSkill, onCancelSkill);

        onEndSkill += () =>
        {
            OnEndSkill();
        };

        bh.mainRullet.PauseRullet();

        GameObject createSkill = null;

        float rand = UnityEngine.Random.Range(0, 100);

        if (rand < 45)
        {
            createSkill = magicObjs[0];
        }
        else if (rand < (45) + 20)
        {
            createSkill = magicObjs[1];
        }
        else if (rand < (45 + 20) + 20)
        {
            createSkill = magicObjs[2];
        }
        else
        {
            createSkill = magicObjs[3];
        }

        GameManager.Instance.inventoryHandler.CreateSkill(createSkill, playerInven, playerInven.transform.position);

        GameManager.Instance.shakeHandler.ShakeBackCvsUI(1f, 0.2f);
        magicParticle.Play();

        GameManager.Instance.animHandler.GetAnim(AnimName.SkillEffect01)
        .SetPosition(transform.position + Vector3.up)
        .SetRotation(Vector3.forward * -90f)
        .SetScale(1.5f)
        .Play(() =>
        {
            ui.ShowMessege("조각 생성!");

            onEndSkill?.Invoke();
        });
    }
}

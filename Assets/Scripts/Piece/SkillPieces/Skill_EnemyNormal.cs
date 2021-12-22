using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EnemyNormal : SkillPiece
{
    public GameObject attackPrefab;
    public GameObject attackExpPrefab;

    private void Awake()
    {
        isPlayerSkill = false;
    }

    public override void Cast()
    {
        print($"적 스킬 발동!! 이름 : {PieceName}");
        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Vector3 target = GameManager.Instance.battleHandler.player.transform.position;
        Vector3 startPos = skillImg.transform.position;

        for (int i = 0; i < 3; i++)
        {
            EffectObj attackObj = Instantiate(attackPrefab, null).GetComponent<EffectObj>();
            attackObj.transform.position = startPos;

            attackObj.Play(target, () =>
            {
                attackObj.GetComponent<SpriteRenderer>().DOFade(0f, 0.1f);
                Instantiate(attackExpPrefab, attackObj.transform.position, Quaternion.identity);
                GameManager.Instance.cameraHandler.ShakeCamera(0.25f, 0.2f);
            }
            , BezierType.Cubic, i * 0.1f);

        }

        GameManager.Instance.battleHandler.player.GetDamage(Value);
    }
}

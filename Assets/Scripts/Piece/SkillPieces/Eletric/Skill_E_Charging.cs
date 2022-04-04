using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Skill_E_Charging : SkillPiece
{
    private BattleHandler bh = null;
    private Action<SkillPiece> onCharge = null;

    public Text counterText;
    private int attackCount = 1;

    private readonly WaitForSeconds pOneSecWait = new WaitForSeconds(0.1f);

    protected override void Start()
    {
        base.Start();

        bh = GameManager.Instance.battleHandler;

        hasTarget = false;
    }

    public override void OnRullet()
    {
        GameManager.Instance.battleHandler.battleEvent.RemoveCastPiece(onCharge);

        onCharge = piece =>
        {
            if (piece.currentType.Equals(PatternType.Diamonds) && piece != this)
            {
                Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
                textEffect.SetType(TextUpAnimType.Damage);
                textEffect.transform.position = skillImg.transform.position;
                textEffect.SetScale(0.8f);
                textEffect.Play("충전됨!");

                LogCon log = new LogCon();
                log.text = $"충전됨";
                log.selfSpr = skillImg.sprite;

                DebugLogHandler.AddLog(LogType.ImageText, log);

                Anim_E_Static_Stun staticEffect = PoolManager.GetItem<Anim_E_Static_Stun>();
                staticEffect.transform.position = skillImg.transform.position;
                staticEffect.SetScale(0.8f);
                staticEffect.Play();

                HighlightColor(0.2f);

                attackCount++;
                counterText.text = attackCount.ToString();
            }
        };

        GameManager.Instance.battleHandler.battleEvent.SetCastPiece(onCharge);
    }

    public override void ResetPiece()
    {
        base.ResetPiece();

        GameManager.Instance.battleHandler.battleEvent.RemoveCastPiece(onCharge);

        attackCount = 1;
        counterText.text = attackCount.ToString();
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        GameManager.Instance.battleHandler.battleUtil.StartCoroutine(ChargeAttack(onCastEnd));
    }

    private IEnumerator ChargeAttack(Action onCastEnd = null)
    {
        if (bh.enemys.Count <= 0)
        {
            onCastEnd?.Invoke();
            yield break;
        }

        List<EnemyHealth> enemys = bh.battleUtil.DeepCopyList(bh.enemys);

        int atkCnt = attackCount;

        for (int i = 0; i < atkCnt; i++)
        {
            EnemyHealth enemy = enemys[Random.Range(0, enemys.Count)];

            enemy.GetDamage(Value, currentType);

            LogCon log = new LogCon();
            log.text = $"{Value} 데미지 부여";
            log.selfSpr = skillImg.sprite;
            log.targetSpr = enemy.GetComponent<SpriteRenderer>().sprite;

            DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);

            Anim_E_Static staticEffect = PoolManager.GetItem<Anim_E_Static>();
            staticEffect.transform.position = enemy.transform.position;
            staticEffect.SetScale(0.8f);

            GameManager.Instance.cameraHandler.ShakeCamera(1.5f, 0.15f);

            staticEffect.Play();

            yield return pOneSecWait;
        }

        attackCount = 1;

        onCastEnd?.Invoke();
    }
}

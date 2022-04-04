using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Skill_E_ComputerError : SkillPiece
{
    private BattleHandler bh = null;

    protected override void Start()
    {
        base.Start();

        hasTarget = false;

        bh = GameManager.Instance.battleHandler;
    }

    public override void Cast(LivingEntity target, Action onCastEnd = null)
    {
        List<EnemyHealth> enemys = bh.battleUtil.DeepCopyList(bh.enemys);

        for (int i = 0; i < enemys.Count; i++)
        {
            Anim_E_Static_Stun stunEffect = PoolManager.GetItem<Anim_E_Static_Stun>();
            stunEffect.transform.position = enemys[i].transform.position;
            stunEffect.SetScale(0.7f);

            stunEffect.Play();

            enemys[i].GetDamage(Value, currentType);

            LogCon log = new LogCon();
            log.text = $"{Value} 데미지 부여";
            log.selfSpr = skillImg.sprite;
            log.targetSpr = enemys[i].GetComponent<SpriteRenderer>().sprite;

            DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);
        }

        GameManager.Instance.cameraHandler.ShakeCamera(0.5f, 0.15f);

        Rullet rullet = bh.mainRullet;
        List<RulletPiece> pieces = rullet.GetPieces();

        List<int> pieceIdxs = new List<int>() { 0, 1, 2, 3, 4, 5 };

        for (int i = 0; i < 2; i++)
        {
            int a = i;

            int randIdx = 0;
            int pieceIdx = 0;
            bool stop = false;

            do
            {
                if (pieceIdxs.Count <= 0)
                {
                    stop = true;
                    break;
                }

                randIdx = Random.Range(0, pieceIdxs.Count);
                pieceIdx = pieceIdxs[randIdx];

                pieceIdxs.RemoveAt(randIdx);
            }
            while (pieces[pieceIdx] == null || pieces[pieceIdx] == this);

            if (stop) break;

            Vector3 effectPos = pieces[pieceIdx].skillImg.transform.position;

            Anim_TextUp textEffect = PoolManager.GetItem<Anim_TextUp>();
            textEffect.SetType(TextUpAnimType.Damage);
            textEffect.transform.position = effectPos;
            textEffect.SetScale(0.8f);
            textEffect.Play("전산오류 효과발동!");


            LogCon log = new LogCon();
            log.text = $"무덤으로 보냄";
            log.selfSpr = skillImg.sprite;
            log.targetSpr = pieces[pieceIdx].skillImg.sprite;

            DebugLogHandler.AddLog(LogType.ImgTextToTarget, log);


            GameManager.Instance.shakeHandler.ShakeBackCvsUI(0.5f, 0.25f);

            Anim_E_Static_Stun stunEffect = PoolManager.GetItem<Anim_E_Static_Stun>();
            stunEffect.transform.position = effectPos;
            stunEffect.SetScale(0.7f);

            stunEffect.Play(() => {
                if(a == 1)
                    onCastEnd?.Invoke();
            });

            pieces[pieceIdx].HighlightColor(0.4f);

            bh.battleUtil.SetPieceToGraveyard(pieceIdx);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BattleHandler : MonoBehaviour
{
    // 턴 룰렛을 돌린다

    // 플레이어가 나온다 -> 플레이어가 걸릴 확률 하락
    // 플레이어의 스킬 룰렛을 돌린다 ->
    // 메인 공격과 서브 공격이 나올 때까지 기다린다
    // 전부 나왔다면 메인 공격과 서브 공격을 한다
    // 공격이 전부 끝났다면 다시 턴 룰렛

    // 적이 나온다 -> 플레이어가 걸릴 확률 증가
    // 적의 공격 중 하나를 랜덤으로 쓴다(or 패턴대로)
    // 적의 공격이 전부 끝났다면 다시 턴 룰렛

    // 탭
    public CanvasGroup tapGroup;

    public Image borderImg;
    public Image subBorderImg;
    public Transform stageBG;

    public Color tapColor;

    // 턴 룰렛
    [Header("룰렛들")]
    public Rullet turnRullet;

    // 메인, 서브 or 나중에 추가될지도 모르는 룰렛
    public List<Rullet> playerRullets = new List<Rullet>();
    // 결과로 나온 룰렛조각들
    public List<RulletPiece> results = new List<RulletPiece>();

    [Space(10f)]
    public PlayerAttack player;
    public EnemyAttack enemy;

    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    private EnemyReward enemyReward;

    private bool isTap = false;
    public bool IsTap => isTap;

    private int rerollCnt = 3;
    private bool canReroll = false;

    private Tween blinkTween;

    private void Awake()
    {
        GameManager.Instance.battleHandler = this;

        playerHealth = player.GetComponent<PlayerHealth>();

        enemyHealth = enemy.GetComponent<EnemyHealth>();
        enemyReward = enemy.GetComponent<EnemyReward>();
    }


    private void Start()
    {
        tapGroup.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!isTap)
            {
                StopPlayerRullet();
            }
            else if (canReroll)
            {
                ReRoll();
            }
        });
    }

    private void ReRoll()
    {
        rerollCnt--;

        blinkTween.Kill();
        tapGroup.GetComponent<Image>().color = tapColor;
        blinkTween = tapGroup.GetComponent<Image>().DOColor(Color.black, 1f)
            .SetEase(Ease.Flash, 10 + 10 - rerollCnt * 3f, 0)
            .SetLoops(-1);

        if (rerollCnt <= 0)
        {
            blinkTween.Kill();
            tapGroup.GetComponent<Image>().color = Color.black;

            canReroll = false;
        }

        for (int i = 0; i < playerRullets.Count; i++)
        {
            playerRullets[i].ReRoll();
        }
    }

    private void StopPlayerRullet()
    {
        for (int i = 0; i < playerRullets.Count; i++)
        {
            playerRullets[i].StopRullet();
        }

        StartCoroutine(CheckTurn());
    }

    private void RollAllRullet()
    {
        for (int i = 0; i < playerRullets.Count; i++)
        {
            playerRullets[i].RollRullet();
        }

        turnRullet.RollRullet();
    }

    private bool CheckRullet()
    {
        for (int i = 0; i < playerRullets.Count; i++)
        {
            if (playerRullets[i].IsRoll)
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator CheckTurn()
    {
        isTap = true;
        canReroll = true;

        rerollCnt = 3;
        tapGroup.transform.GetChild(0).transform.DOLocalMoveY(-180f, 0.2f);

        blinkTween = tapGroup.GetComponent<Image>().DOColor(Color.black, 1f)
            .SetEase(Ease.Flash, 10, 0)
            .SetLoops(-1);

        // 전부 돌릴 때까지

        yield return new WaitUntil(CheckRullet);
        canReroll = false;

        blinkTween.Kill();
        tapGroup.GetComponent<Image>().color = Color.black;

        //yield return new WaitForSeconds(0.5f);

        turnRullet.StopRullet();

        yield return new WaitUntil(() => !turnRullet.IsRoll);
        yield return new WaitForSeconds(1f);

        ResetRullets();

        if (enemyHealth.IsDie)
        {
            yield return new WaitUntil(() => !enemyReward.IsReward);

            GoNextRoom();
            yield return new WaitForSeconds(2f);
            enemyHealth.Revive();

            yield return new WaitUntil(() => !enemyHealth.IsDie);

            (turnRullet as TurnRullet).InitTurn();
        }

        yield return new WaitForSeconds(0.5f);
        RollAllRullet();

        yield return new WaitForSeconds(0.1f);

        tapGroup.GetComponent<Image>().DOColor(tapColor, 0.2f);
        tapGroup.transform.GetChild(0).transform.DOLocalMoveY(0f, 0.2f);

        isTap = false;
    }

    private void GoNextRoom()
    {
        Vector3 size = new Vector3(0f, 0f, 0f);
        float dur = 0.7f;
        float moveY = 0.25f;

        /*
        Sequence moveEffect = DOTween.Sequence()
            .Append(stageBG.transform.DOMoveY(moveY, dur).SetRelative())
            .Join(stageBG.DOScale(size, dur).SetRelative())
            .Append(stageBG.transform.DOMoveY(-moveY, dur).SetRelative())
            .Join(stageBG.DOScale(-size, dur).SetRelative())
            .Append(stageBG.transform.DOMoveY(moveY, dur).SetRelative())
            .Join(stageBG.DOScale(size, dur).SetRelative())
            .Append(stageBG.transform.DOMoveY(-moveY, dur).SetRelative())
            .Join(stageBG.DOScale(-size, dur).SetRelative())
            .AppendInterval(0.5f)
            .AppendCallback(() => 
            {
                enemyHealth.Revive();
            });
        */
    }

    private void ResetRullets()
    {
        for (int i = 0; i < playerRullets.Count; i++)
        {
            playerRullets[i].ResetRulletSize();
        }

        turnRullet.ResetRulletSize();

        borderImg.DOColor(Color.white, 0.55f);
        subBorderImg.DOColor(Color.white, 0.55f);

        borderImg.GetComponent<RotateBorder>().SetSpeed(false);
        subBorderImg.GetComponent<RotateBorder>().SetSpeed(false);
    }

    public void PlayerAttack()
    {
        player.AttackSkill(results, enemyHealth);
        results.Clear();
    }

    public void EnemyAttack()
    {
        enemy.Attack(playerHealth);
        results.Clear();
    }
}

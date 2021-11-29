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
    private EnemyHealth enemyHealth;

    private void Awake()
    {
        GameManager.Instance.battleHandler = this;

        enemyHealth = enemy.GetComponent<EnemyHealth>();
    }

    private void Start()
    {
        tapGroup.GetComponent<Button>().onClick.AddListener(() =>
        {
            tapGroup.alpha = 0f;
            tapGroup.interactable = false;
            tapGroup.blocksRaycasts = false;

            RollPlayerRullet();
        });
    }

    private void RollPlayerRullet()
    {
        for (int i = 0; i < playerRullets.Count; i++)
        {
            playerRullets[i].RollRullet();
        }

        StartCoroutine(CheckTurn());
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
        // 전부 돌릴 때까지
        yield return new WaitUntil(CheckRullet);
        //yield return new WaitForSeconds(0.5f);

        turnRullet.RollRullet();

        yield return new WaitUntil(() => !turnRullet.IsRoll);
        yield return new WaitForSeconds(1f);

        ResetRullets();

        if (enemyHealth.IsDie)
        {
            yield return new WaitUntil(() => !enemyHealth.enemyReward.IsReward);

            GoNextRoom();

            yield return new WaitUntil(() => !enemyHealth.IsDie);

            (turnRullet as TurnRullet).InitTurn();
        }

        tapGroup.alpha = 1f;
        tapGroup.interactable = true;
        tapGroup.blocksRaycasts = true;
    }

    private void GoNextRoom()
    {
        Vector3 size = new Vector3(0f, 0f, 0f);
        float dur = 0.7f;
        float moveY = 0.25f;

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
        player.AttackSkill(results);
        results.Clear();
    }

    public void EnemyAttack()
    {
        enemy.Attack();
        results.Clear();
    }
}

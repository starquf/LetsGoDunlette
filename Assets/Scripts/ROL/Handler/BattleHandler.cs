using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class BattleHandler : MonoBehaviour
{
    // �� �귿�� ������

    // �÷��̾ ���´� -> �÷��̾ �ɸ� Ȯ�� �϶�
    // �÷��̾��� ��ų �귿�� ������ ->
    // ���� ���ݰ� ���� ������ ���� ������ ��ٸ���
    // ���� ���Դٸ� ���� ���ݰ� ���� ������ �Ѵ�
    // ������ ���� �����ٸ� �ٽ� �� �귿

    // ���� ���´� -> �÷��̾ �ɸ� Ȯ�� ����
    // ���� ���� �� �ϳ��� �������� ����(or ���ϴ��)
    // ���� ������ ���� �����ٸ� �ٽ� �� �귿

    // ��
    public CanvasGroup tapGroup;

    public Image borderImg;
    public Image subBorderImg;
    public Transform stageBG;

    // �� �귿
    [Header("�귿��")]
    public Rullet turnRullet;

    // ����, ���� or ���߿� �߰������� �𸣴� �귿
    public List<Rullet> playerRullets = new List<Rullet>();
    // ����� ���� �귿������
    public List<RulletPiece> results = new List<RulletPiece>();

    [Space(10f)]
    public PlayerAttack player;
    public EnemyAttack enemy;

    private PlayerHealth playerHealth;
    private EnemyHealth enemyHealth;

    private EnemyReward enemyReward;

    private bool isTap = false;
    public bool IsTap => isTap;

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
            tapGroup.alpha = 0f;
            tapGroup.interactable = false;
            tapGroup.blocksRaycasts = false;

            StopPlayerRullet();
        });
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

        // ���� ���� ������
        yield return new WaitUntil(CheckRullet);
        //yield return new WaitForSeconds(0.5f);

        turnRullet.StopRullet();

        yield return new WaitUntil(() => !turnRullet.IsRoll);
        yield return new WaitForSeconds(1f);

        ResetRullets();

        if (enemyHealth.IsDie)
        {
            yield return new WaitUntil(() => !enemyReward.IsReward);

            GoNextRoom();

            yield return new WaitUntil(() => !enemyHealth.IsDie);

            (turnRullet as TurnRullet).InitTurn();
        }

        yield return new WaitForSeconds(0.5f);
        RollAllRullet();

        yield return new WaitForSeconds(0.1f);

        tapGroup.alpha = 1f;
        tapGroup.interactable = true;
        tapGroup.blocksRaycasts = true;

        isTap = false;
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
        player.AttackSkill(results, enemyHealth);
        results.Clear();
    }

    public void EnemyAttack()
    {
        enemy.Attack(playerHealth);
        results.Clear();
    }
}

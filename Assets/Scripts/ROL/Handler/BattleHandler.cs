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
        // ���� ���� ������
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

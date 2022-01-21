using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BattleRewardHandler : MonoBehaviour
{
    private List<GameObject> rewards = new List<GameObject>();

    private InventoryHandler inventoryHandler;
    private BattleHandler battleHandler;

    public BattleRewardUIHandler battleRewardUIHandler;
    private bool isWinEffectEnd;
    private bool isRewardEnd;

    #region WaitForSeconds

    private readonly WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    #endregion

    private void Start()
    {
        inventoryHandler = GameManager.Instance.inventoryHandler;
        battleHandler = GameManager.Instance.battleHandler;
    }

    public void Init(List<GameObject> rewards)
    {
        this.rewards = rewards;
    }

    public void GiveReward()
    {
        StartCoroutine(RewardRoutine());
    }

    private IEnumerator RewardRoutine()
    {
        PutRulletPieceInInventory(); //�귿 �� �κ��丮�� �ְ�
        yield return oneSecWait;
        yield return oneSecWait;
        inventoryHandler.RemoveAllEnemyPiece(); //�� �귿 ���ְ�

        battleRewardUIHandler.ShowWinEffect(() => { isWinEffectEnd = true; });
        yield return new WaitUntil(()=>isWinEffectEnd);

        StartCoroutine(CreateReward());
        yield return new WaitUntil(()=> isRewardEnd); //�� ������ �ְ�

        // ��

        // ���� �ٽ� ���� �ӽ÷� �־��
        battleHandler.StartBattle();
    }

    private IEnumerator CreateReward()
    {
        SkillRullet rullet = battleHandler.mainRullet;
        Transform rulletParent = rullet.transform.parent; // �귿 ���� �θ� Ʈ������
        SkillPiece rewardResult = new SkillPiece();
        int rewardResultIdx = -1;

        rullet.transform.SetParent(battleRewardUIHandler.transform);
        rullet.transform.SetAsFirstSibling();

        for (int i = 0; i < 6; i++)
        {
            SkillPiece skill = Instantiate(rewards[Random.Range(0, rewards.Count)], rullet.transform).GetComponent<SkillPiece>();
            rullet.AddPiece(skill);
        }
        rullet.SetRulletSmooth();

        yield return new WaitForSeconds(0.5f);

        rullet.RollRullet();
        yield return new WaitForSeconds(2f);

        bool isRulletStop = false;
        // �귿 ���缭 ��� �޾ƿ��°�
        rullet.StopRullet((result, pieceIdx) =>
        {
            rullet.HighlightResult();

            rewardResult = result as SkillPiece;
            rewardResultIdx = pieceIdx;
            isRulletStop = true;
        });

        yield return new WaitUntil(() => isRulletStop);
        List<RulletPiece> rulletPieces = rullet.GetPieces();
        for (int i = 0; i < 6; i++)
        {
            if (i != rewardResultIdx)
            {
                Destroy(rulletPieces[i].gameObject);
            }
            rullet.SetEmpty(i);
        }
        yield return oneSecWait;

        DOTween.Sequence().Append(rewardResult.transform.DOMove(rullet.transform.position+Vector3.down*0.7f, 0.5f))
            .Join(rewardResult.transform.DORotate(Quaternion.Euler(0,0,30).eulerAngles, 0.5f))
            .OnComplete(() =>
            {
                battleRewardUIHandler.SetButton(() =>
                {
                    print("������ ������");
                    Inventory owner = battleHandler.player.GetComponent<Inventory>();
                    rewardResult.gameObject.SetActive(false);
                    rewardResult.owner = owner;
                    GameManager.Instance.inventoryHandler.AddSkill(rewardResult);

                    rullet.transform.SetParent(rulletParent);
                    rullet.transform.SetAsFirstSibling();

                    isRewardEnd = true;
                },()=>
                {
                    print("������ ����");
                    Destroy(rewardResult.gameObject);

                    rullet.transform.SetParent(rulletParent);
                    rullet.transform.SetAsFirstSibling();

                    isRewardEnd = true;
                });
            })
        ;
    }

    private void PutRulletPieceInInventory() //�귿�� �κ��丮�� �ٳ�������.
    {
        inventoryHandler.CycleSkills(); //���� > �κ�

        battleHandler.mainRullet.PutAllRulletPieceToInventory(); //�귿 > �κ�
    }
}

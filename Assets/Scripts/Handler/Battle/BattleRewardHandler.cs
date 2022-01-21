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
        PutRulletPieceInInventory(); //룰렛 다 인벤토리에 넣고
        yield return oneSecWait;
        yield return oneSecWait;
        inventoryHandler.RemoveAllEnemyPiece(); //적 룰렛 없애고

        battleRewardUIHandler.ShowWinEffect(() => { isWinEffectEnd = true; });
        yield return new WaitUntil(()=>isWinEffectEnd);

        StartCoroutine(CreateReward());
        yield return new WaitUntil(()=> isRewardEnd); //적 보상을 주고

        // 끝

        // 전투 다시 시작 임시로 넣어둠
        battleHandler.StartBattle();
    }

    private IEnumerator CreateReward()
    {
        SkillRullet rullet = battleHandler.mainRullet;
        Transform rulletParent = rullet.transform.parent; // 룰렛 기존 부모 트랜스폼
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
        // 룰렛 멈춰서 결과 받아오는거
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
                    print("리워드 가져감");
                    Inventory owner = battleHandler.player.GetComponent<Inventory>();
                    rewardResult.gameObject.SetActive(false);
                    rewardResult.owner = owner;
                    GameManager.Instance.inventoryHandler.AddSkill(rewardResult);

                    rullet.transform.SetParent(rulletParent);
                    rullet.transform.SetAsFirstSibling();

                    isRewardEnd = true;
                },()=>
                {
                    print("리워드 버림");
                    Destroy(rewardResult.gameObject);

                    rullet.transform.SetParent(rulletParent);
                    rullet.transform.SetAsFirstSibling();

                    isRewardEnd = true;
                });
            })
        ;
    }

    private void PutRulletPieceInInventory() //룰렛을 인벤토리에 다넣으세요.
    {
        inventoryHandler.CycleSkills(); //무덤 > 인벤

        battleHandler.mainRullet.PutAllRulletPieceToInventory(); //룰렛 > 인벤
    }
}

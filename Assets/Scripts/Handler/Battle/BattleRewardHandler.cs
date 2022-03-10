using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

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

    public void ResetRullet(Action action)
    {
        StartCoroutine(ResetInventory(action));
    }

    public IEnumerator ResetInventory(Action action)
    {
        PutRulletPieceInInventory(); //·ê·¿ ´Ù ÀÎº¥Åä¸®¿¡ ³Ö°í
        yield return oneSecWait;
        yield return oneSecWait;
        inventoryHandler.RemoveAllEnemyPiece(); //Àû ·ê·¿ ¾ø¾Ö°í

        action();
    }

    private IEnumerator RewardRoutine()
    {
        yield return null;

        PutRulletPieceInInventory(); //·ê·¿ ´Ù ÀÎº¥Åä¸®¿¡ ³Ö°í
        yield return oneSecWait;
        yield return oneSecWait;
        inventoryHandler.RemoveAllEnemyPiece(); //Àû ·ê·¿ ¾ø¾Ö°í

        battleRewardUIHandler.ShowWinEffect(() => { isWinEffectEnd = true; });
        yield return new WaitUntil(()=>isWinEffectEnd);

        StartCoroutine(CreateReward());
        yield return new WaitUntil(()=> isRewardEnd); //Àû º¸»óÀ» ÁÖ°í

        // ÀüÅõ ³¡ ¾Ë¸²
        GameManager.Instance.EndEncounter();
        isRewardEnd = false;
        isWinEffectEnd = false;
    }

    private IEnumerator CreateReward()
    {
        SkillRullet rullet = battleHandler.mainRullet;
        Transform rulletParent = rullet.transform.parent; // ·ê·¿ ±âÁ¸ ºÎ¸ð Æ®·£½ºÆû
        SkillPiece rewardResult = null;
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

        rullet.RollRullet(false);
        yield return new WaitForSeconds(2f);

        bool isRulletStop = false;
        // ·ê·¿ ¸ØÃç¼­ °á°ú ¹Þ¾Æ¿À´Â°Å

        rullet.onResult = (result, pieceIdx) =>
        {
            rullet.HighlightResult();

            rewardResult = result as SkillPiece;
            rewardResultIdx = pieceIdx;
            isRulletStop = true;
        };

        rullet.StopRullet();

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
                rewardResult.transform.SetParent(battleRewardUIHandler.transform);
                rewardResult.transform.SetAsFirstSibling();

                rullet.transform.SetParent(rulletParent);
                rullet.transform.SetAsFirstSibling();


                battleRewardUIHandler.SetButton(rewardResult, () =>
                {
                    print("¸®¿öµå °¡Á®°¨");
                    Transform unusedInventoryTrm = GameManager.Instance.inventoryHandler.transform;
                    DOTween.Sequence().Append(rewardResult.transform.DOMove(unusedInventoryTrm.position, 0.5f))
                    .Join(rewardResult.transform.DOScale(Vector2.one * 0.1f, 0.5f))
                    .Join(rewardResult.GetComponent<Image>().DOFade(0f,0.5f))
                    .OnComplete(()=>
                    {
                        Inventory owner = battleHandler.player.GetComponent<Inventory>();
                        rewardResult.gameObject.SetActive(false);
                        rewardResult.owner = owner;
                        GameManager.Instance.inventoryHandler.AddSkill(rewardResult);
                        rewardResult.GetComponent<Image>().color = Color.white;

                        isRewardEnd = true;
                    });
                },()=>
                {
                    print("¸®¿öµå ¹ö¸²");

                    DOTween.Sequence().Append(rewardResult.transform.DOMoveX(3f, 0.5f))
                    .Join(rewardResult.transform.DORotate(Quaternion.Euler(0,0,-60).eulerAngles, 0.5f))
                    .Join(rewardResult.GetComponent<Image>().DOFade(0f, 0.5f))
                    .OnComplete(() =>
                    {
                        Destroy(rewardResult.gameObject);


                        print("³¡");
                        isRewardEnd = true;
                    });
                });
            })
        ;
    }

    private void PutRulletPieceInInventory() //·ê·¿À» ÀÎº¥Åä¸®¿¡ ´Ù³ÖÀ¸¼¼¿ä.
    {
        inventoryHandler.CycleSkills(); //¹«´ý > ÀÎº¥

        battleHandler.mainRullet.PutAllRulletPieceToInventory(); //·ê·¿ > ÀÎº¥
    }
}

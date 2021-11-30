using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class EnemyReward : MonoBehaviour
{
    public Transform rulletTrans;

    private bool isReward = false;
    public bool IsReward => isReward;



    public void GiveReward() // 매개변수가 달라서 일단박아놈
    {
        isReward = true;

        int rand = UnityEngine.Random.Range(0, GameManager.Instance.rewardObjs.Count);
        GameObject result = GameManager.Instance.rewardObjs[rand];

        print(result);
        print(result.GetComponent<SkillPiece>());
        print(result.GetComponent<Image>().sprite);

        GameObject reward = Instantiate(result, transform.position, Quaternion.identity, rulletTrans);

        reward.transform.localPosition = new Vector3(reward.transform.localPosition.x, reward.transform.localPosition.y, 0f);
        reward.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        reward.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

        Sequence rewardSeq = DOTween.Sequence()
            .Append(reward.GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.Linear))
            .Append(reward.transform.DOMove((Vector2)GameManager.Instance.inventoryHandler.OpenBtn.transform.position, 1f))
            .AppendCallback(() =>
            {
                Destroy(reward);
                GameManager.Instance.OnReward(result.GetComponent<SkillPiece>(), result.GetComponent<Image>().sprite);
                isReward = false;
            });
        

        //룰렛에 들어가는 부분 주석처리 인벤토리로 옮김
        //GameObject reward = Instantiate(rewardObjs[rand], transform.position, Quaternion.identity, rulletTrans);

        //reward.transform.localPosition = new Vector3(reward.transform.localPosition.x, reward.transform.localPosition.y, 0f);
        //reward.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        //reward.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

        //Sequence rewardSeq = DOTween.Sequence()
        //    .Append(reward.GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.Linear))
        //    .AppendCallback(() =>
        //    {
        //        rulletTrans.GetComponent<Rullet>().AddPiece(reward.GetComponent<RulletPiece>());
        //    })
        //    .Append(reward.transform.DOLocalMove(Vector3.zero, 1f))
        //    .AppendCallback(() =>
        //    {
        //        isReward = false;
        //    });
    }
}

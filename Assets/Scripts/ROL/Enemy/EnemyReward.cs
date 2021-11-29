using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyReward : MonoBehaviour
{
    public List<GameObject> rewardObjs;
    public Transform rulletTrans;

    private bool isReward = false;
    public bool IsReward => isReward;

    public void GiveReward()
    {
        isReward = true;

        int rand = Random.Range(0, rewardObjs.Count);

        GameObject reward = Instantiate(rewardObjs[rand], transform.position, Quaternion.identity, rulletTrans);

        reward.transform.localPosition = new Vector3(reward.transform.localPosition.x, reward.transform.localPosition.y, 0f);
        reward.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        reward.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0f);

        Sequence rewardSeq = DOTween.Sequence()
            .Append(reward.GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.Linear))
            .AppendCallback(() =>
            {
                rulletTrans.GetComponent<Rullet>().AddPiece(reward.GetComponent<RulletPiece>());
            })
            .Append(reward.transform.DOLocalMove(Vector3.zero, 1f))
            .AppendCallback(() => 
            {
                isReward = false;
            });
    }
}

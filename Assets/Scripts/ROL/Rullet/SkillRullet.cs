using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillRullet : Rullet
{
    public GameObject nomalRulletPrefab;
    public Image borderImg;
    private bool isRollingReward = false;

    protected override void Start()
    {
        GetComponentsInChildren(pieces);

        SetRullet();
        int sizeSum = 0;
        for (int i = 0; i < pieces.Count; i++)
        {
            sizeSum += pieces[i].Size;
        }
        int addNomalCnt = (maxSize - sizeSum) / 6;
        AddNormalAttackPiece(addNomalCnt);
        RollRullet();
    }

    public void StopRulletToChangePiece()
    {
        isRollingReward = true;
        StopRullet();
    }

    public void ChangePiece(int changeIdx, RulletPiece changePiece)
    {
        Destroy(pieces[changeIdx].gameObject);
        pieces[changeIdx] = changePiece;
        SetRulletSmooth();
    }


    public void AddNormalAttackPiece(int addCnt)
    {
        for (int i = 0; i < addCnt; i++)
        {
            GameObject nomalRullet = Instantiate(nomalRulletPrefab, Vector3.zero, Quaternion.identity, gameObject.transform);
            pieces.Add(nomalRullet.GetComponent<RulletPiece>());
            nomalRullet.transform.localPosition = Vector3.zero;
        }
        SetRullet();
    }

    protected override void RulletResult()
    {
        // 총 각도의 합
        float angleSum = 0f;
        // 크기 1당 각도
        float sizeAngle = 360f / maxSize;

        // -180 ~ 180 표현식을 0 ~ 360 표현식으로 변환하는 과정
        float rulletAngle = Quaternion.FromToRotation(Vector3.right, transform.right).eulerAngles.z;
        //print("룰렛 각도 : " + rulletAngle);

        int pieceIdx = -1;

        for (int i = 0; i < pieces.Count; i++)
        {
            // 범위 안에 있다면
            if (rulletAngle <= angleSum + pieces[i].Size * sizeAngle
                && rulletAngle >= angleSum)
            {
                pieceIdx = i;
                break;
            }

            // 시작 범위를 해당 조각의 사이즈만큼 증가
            angleSum += pieces[i].Size * sizeAngle;
        }


        if (pieceIdx >= 0)
        {
            print($"{pieceIdx + 1}번째 당첨!");
            result = pieces[pieceIdx];
        }
        else
        {
            //print("기본 당첨!");
            CastDefault();
        }

        if (!isRollingReward)
        {
            fillTween = result.GetComponent<Image>().DOFillAmount(1f, 0.6f);
            result.transform.SetAsLastSibling();
            result.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.55f);
            result.Highlight();

            if (result != null)
            {
                borderImg.DOColor(result.Color, 0.55f);
                borderImg.GetComponent<RotateBorder>().SetSpeed(true);
            }

            GameManager.Instance.battleHandler.results.Add(result);
        }
        else
        {
            GameManager.Instance.inventoryHandler.result = result;
            GameManager.Instance.inventoryHandler.resultIdx = pieceIdx;
        }
    }

    protected override void CastDefault()
    {
        result = null;
    }
}

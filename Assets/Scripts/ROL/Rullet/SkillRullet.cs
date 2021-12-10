using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SkillRullet : Rullet
{
    // 기본 룰렛 조각 프리팹
    public GameObject nomalRulletPrefab;
    public Image borderImg;

    // 장착하고 있는 중인가?
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

    // 바꾸기 위해서 룰렛을 멈출 때 불리는 함수
    public void StopRulletToChangePiece()
    {
        isRollingReward = true;
        StopRullet();
    }

    // 해당 인덱스의 룰렛을 바꾸는 함수
    public void ChangePiece(int changeIdx, RulletPiece changePiece)
    {
        Destroy(pieces[changeIdx].gameObject);
        pieces[changeIdx] = changePiece;
        SetRulletSmooth();

        isRollingReward = false;
    }

    // cnt만큼 기본 룰렛조각 추가해주는 함수
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
        float angleSum = 0f;
        float sizeAngle = 360f / maxSize;

        float rulletAngle = Quaternion.FromToRotation(Vector3.right, transform.right).eulerAngles.z;

        int pieceIdx = -1;

        for (int i = 0; i < pieces.Count; i++)
        {
            if (rulletAngle <= angleSum + pieces[i].Size * sizeAngle
                && rulletAngle >= angleSum)
            {
                pieceIdx = i;
                break;
            }

            angleSum += pieces[i].Size * sizeAngle;
        }

        if (pieceIdx >= 0)
        {
            print($"{pieceIdx + 1} selected!");
            result = pieces[pieceIdx];
            borderImg.DOColor(result.Color, 0.55f);
            borderImg.GetComponent<RotateBorder>().SetSpeed(true);

            transform.DOShakePosition(0.2f, 15f, 50);
        }
        else
        {
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

            ComboManager.Instance.AddComboQueue(result);
            GameManager.Instance.battleHandler.results.Add(result);
        }
        else
        {
            // 이것도 함수 하나로 빼자
            GameManager.Instance.inventoryHandler.result = result;
            GameManager.Instance.inventoryHandler.resultIdx = pieceIdx;
        }
    }

    protected override void CastDefault()
    {
        result = null;
    }
}

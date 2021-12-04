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
        // �� ������ ��
        float angleSum = 0f;
        // ũ�� 1�� ����
        float sizeAngle = 360f / maxSize;

        // -180 ~ 180 ǥ���� 0 ~ 360 ǥ������ ��ȯ�ϴ� ���
        float rulletAngle = Quaternion.FromToRotation(Vector3.right, transform.right).eulerAngles.z;
        //print("�귿 ���� : " + rulletAngle);

        int pieceIdx = -1;

        for (int i = 0; i < pieces.Count; i++)
        {
            // ��� �ȿ� �ִٸ�
            if (rulletAngle <= angleSum + pieces[i].Size * sizeAngle
                && rulletAngle >= angleSum)
            {
                pieceIdx = i;
                break;
            }

            // ���� ����� �ش� ����� �����ŭ ��
            angleSum += pieces[i].Size * sizeAngle;
        }


        if (pieceIdx >= 0)
        {
            print($"{pieceIdx + 1}��° ��÷!");
            result = pieces[pieceIdx];
            borderImg.DOColor(result.Color, 0.55f);
            borderImg.GetComponent<RotateBorder>().SetSpeed(true);
        }
        else
        {
            //print("�⺻ ��÷!");
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
            GameManager.Instance.inventoryHandler.result = result;
            GameManager.Instance.inventoryHandler.resultIdx = pieceIdx;
        }
    }

    protected override void CastDefault()
    {
        result = null;
    }
}

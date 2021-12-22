using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public abstract class Rullet : MonoBehaviour
{
    protected List<RulletPiece> pieces = new List<RulletPiece>();
    protected RulletPiece result;

    protected Action<RulletPiece, int> onResult;

    protected int maxSize = 36;

    private bool isRoll = false;
    public bool IsRoll => isRoll;

    private bool isStop = false;

    protected float rollSpeed;
    protected float stopSpeed;
    protected float multiply = 1f;

    protected Tween fillTween;

    public Transform pinTrans;

    protected virtual void Start()
    {
        GetComponentsInChildren(pieces);

        SetRullet();
    }

    public virtual void ResetRulletSize()
    {
        fillTween.Kill();

        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].GetComponent<Image>().DOFillAmount(pieces[i].Size / (float)maxSize, 0.3f);
            pieces[i].transform.DOScale(Vector3.one, 0.3f);
        }
    }

    public virtual List<RulletPiece> GetPieces()
    {
        return pieces;
    }

    public virtual void AddPiece(RulletPiece piece)
    {
        piece.transform.SetParent(transform);
        piece.transform.DOLocalMove(Vector3.zero, 0.35f);
        piece.transform.DOScale(Vector3.one, 0.35f);

        pieces.Add(piece);

        SetRulletSmooth();
    }

    public virtual void SetRulletSmooth()
    {
        // 총 크기의 합
        int sizeSum = 0;
        // 크기 1당 각도
        float angle = -360f / maxSize;

        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].transform.DORotateQuaternion(Quaternion.AngleAxis(transform.eulerAngles.z + sizeSum * angle, Vector3.forward), 0.35f);

            sizeSum += pieces[i].Size;
        }
    }

    public virtual void SetRullet()
    {
        // 총 크기의 합
        int sizeSum = 0;
        // 크기 1당 각도
        float angle = -360f / maxSize;

        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z + sizeSum * angle, Vector3.forward);

            sizeSum += pieces[i].Size;
        }
    }

    public virtual void RollRullet()
    {
        if (isRoll) return;

        result = null;
        isRoll = true;

        StartCoroutine(Roll());
    }

    public void StopRullet(Action<RulletPiece, int> onResult)
    {
        isStop = true;

        this.onResult = onResult;
    }

    public void ReRoll()
    {
        rollSpeed = (1500f + UnityEngine.Random.Range(0f, 500f)) * UnityEngine.Random.Range(1f, 2f) * multiply;
        stopSpeed = UnityEngine.Random.Range(2f, 2.5f);
    }

    protected virtual IEnumerator Roll()
    {
        rollSpeed = (1500f + UnityEngine.Random.Range(0f, 500f)) * multiply;
        stopSpeed = UnityEngine.Random.Range(2f, 2.5f);

        while (Mathf.Abs(rollSpeed) > 1.5f)
        {
            yield return null;

            transform.Rotate(0f, 0f, rollSpeed * Time.deltaTime);

            if (pinTrans != null)
            {
                // 이새낀 보류해주세요 나중에 선한쌤이 보면 됡것 같습니다
                pinTrans.rotation = Quaternion.AngleAxis(Mathf.Clamp(Mathf.Cos(transform.eulerAngles.z * Mathf.Deg2Rad * 5.5f), -1f, 0f) * 30f, Vector3.forward);
            }

            if (isStop)
            {
                rollSpeed = Mathf.Lerp(rollSpeed, 0f, Time.deltaTime * stopSpeed);
            }
        }

        isStop = false;
        isRoll = false;

        RulletResult(onResult);
    }

    protected virtual void RulletResult(Action<RulletPiece, int> onResult)
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
            print("기본 당첨!");
            CastDefault();
        }

        onResult?.Invoke(result, pieceIdx);
    }

    public virtual void HighlightResult()
    {
        fillTween = result.GetComponent<Image>().DOFillAmount(1f, 0.6f);
        result.transform.SetAsLastSibling();
        result.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.55f);
        result.Highlight();

        transform.DOShakePosition(0.2f, 15f, 50);
    }

    protected abstract void CastDefault();
}
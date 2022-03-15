using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public abstract class Rullet : MonoBehaviour
{
    [SerializeField] protected List<RulletPiece> pieces = new List<RulletPiece>();
    protected RulletPiece result;

    public Action<RulletPiece, int> onResult;

    protected int maxSize = 36;

    private bool isRoll = false;
    public bool IsRoll => isRoll;

    private bool isStop = false;
    public bool IsStop => isStop;

    protected float rollSpeed;
    protected float stopSpeed;
    protected float multiply = 1f;

    protected float rulletSpeed = 0f;
    public float RulletSpeed 
    {
        get 
        {
            return rulletSpeed; 
        }
        set
        {
            rulletSpeed = value;

            rulletSpeed = Mathf.Clamp(rulletSpeed, 500f, 1500f);
            speedText.text = $"Speed : {rulletSpeed.ToString()}";
        }
    }

    protected Coroutine rollCor;
    protected Coroutine timeCor;
    protected Tween fillTween;

    //public Transform pinTrans;
    public Text speedText;
    public Text timeText;

    protected int currentTime;
    protected bool isPaused = false;

    protected WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    protected virtual void Start()
    {
        ResetRulletSpeed();
        SetRullet();
    }

    public virtual void ResetRulletSize()
    {
        fillTween.Kill();

        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] == null) continue;

            pieces[i].GetComponent<Image>().DOFillAmount(pieces[i].Size / (float)maxSize, 0.3f);
            pieces[i].transform.DOScale(Vector3.one, 0.3f);
            pieces[i].UnHighlight();
        }
    }

    public virtual void ResetRulletSpeed()
    {
        rulletSpeed = 600f;
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

        if (pieces.Count >= 6)
        {
            for (int i = 0; i < pieces.Count; i++)
            {
                if (pieces[i] == null) // ��ĭ�̶��
                {
                    print("����ִ� ĭ : " + i);

                    piece.pieceIdx = i;
                    pieces[i] = piece;

                    break;
                }
            }
        }
        else
        {
            piece.pieceIdx = pieces.Count;
            pieces.Add(piece);
        }

        SetRulletSmooth();
    }

    public virtual void SetRulletSmooth()
    {
        // �� ũ���� ��
        int sizeSum = 0;
        // ũ�� 1�� ����
        float angle = -360f / maxSize;

        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] != null)
            {
                pieces[i].transform
                            .DORotateQuaternion(Quaternion.AngleAxis(transform.eulerAngles.z + sizeSum * angle, Vector3.forward), 0.35f);

                sizeSum += pieces[i].Size;
            }
            else
            {
                sizeSum += 6;
            }
        }
    }

    public virtual void SetRullet()
    {
        // �� ũ���� ��
        int sizeSum = 0;
        // ũ�� 1�� ����
        float angle = -360f / maxSize;

        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z + sizeSum * angle, Vector3.forward);

            sizeSum += pieces[i].Size;
        }
    }

    public virtual void RollRullet(bool hasTimer = true, int time = 10)
    {
        if (isRoll) return;

        result = null;
        isRoll = true;
        isStop = false;

        timeText.text = "";

        rollCor = StartCoroutine(Roll());

        if (hasTimer)
        {
            if(!isPaused)
                currentTime = time;

            timeCor = StartCoroutine(Timer());
        }
    }

    public virtual void PauseRullet()
    {
        if (isStop) return;

        if (rollCor != null)
            StopCoroutine(rollCor);

        if(timeCor != null)
            StopCoroutine(timeCor);

        timeText.text = "";

        isPaused = true;
        isRoll = false;
    }

    public void StopRullet()
    {
        isStop = true;

        if (timeCor != null)
            StopCoroutine(timeCor);

        timeText.text = "";
    }

    public void ReRoll()
    {
        rollSpeed = (600f + UnityEngine.Random.Range(0f, 100f)) * multiply;
        stopSpeed = UnityEngine.Random.Range(10f, 10.5f);
    }

    protected virtual IEnumerator Roll()
    {
        rollSpeed = (rulletSpeed + UnityEngine.Random.Range(0, 100)) * multiply;
        stopSpeed = UnityEngine.Random.Range(10f, 10.5f);

        speedText.text = $"Speed : {rulletSpeed.ToString()}";

        while (Mathf.Abs(rollSpeed) > 1.5f)
        {
            yield return null;

            transform.Rotate(0f, 0f, rollSpeed * Time.deltaTime);

            if (isStop)
            {
                rollSpeed = Mathf.Lerp(rollSpeed, 0f, Time.deltaTime * stopSpeed);
            }
        }

        isStop = false;
        isRoll = false;
        isPaused = false;

        RulletResult(onResult);
    }

    protected virtual IEnumerator Timer()
    {
        for (int i = currentTime; i > 0; i--)
        {
            currentTime--;
            timeText.text = i.ToString();

            if (i < 4)
            {
                timeText.color = Color.red;
            }
            else
            {
                timeText.color = Color.white;
            }

            yield return oneSecWait;
        }

        timeText.text = "";

        isStop = true;
    }

    protected virtual void RulletResult(Action<RulletPiece, int> onResult)
    {
        // �� ������ ��
        float angleSum = 0f;
        // ũ�� 1�� ����
        float sizeAngle = 360f / maxSize;

        // -180 ~ 180 ǥ������ 0 ~ 360 ǥ�������� ��ȯ�ϴ� ����
        float rulletAngle = Quaternion.FromToRotation(Vector3.right, transform.right).eulerAngles.z;
        //print("�귿 ���� : " + rulletAngle);

        int pieceIdx = -1;

        for (int i = 0; i < pieces.Count; i++)
        {
            // ���� �ȿ� �ִٸ�
            if (rulletAngle <= angleSum + pieces[i].Size * sizeAngle
                && rulletAngle >= angleSum)
            {
                pieceIdx = i;
                break;
            }

            // ���� ������ �ش� ������ �����ŭ ����
            angleSum += pieces[i].Size * sizeAngle;
        }

        if (pieceIdx >= 0)
        {
            print($"{pieceIdx + 1}��° ��÷!");
            result = pieces[pieceIdx];
        }
        else
        {
            print("�⺻ ��÷!");
            CastDefault();
        }

        onResult?.Invoke(result, pieceIdx);
    }

    public virtual void HighlightResult()
    {
        //fillTween = result.GetComponent<Image>().DOFillAmount(1f, 0.6f);
        result.transform.SetAsLastSibling();
        result.transform.DOScale(new Vector3(1.15f, 1.15f, 1f), 0.55f);
        result.Highlight();

        transform.DOShakePosition(0.2f, 15f, 50);
    }

    protected abstract void CastDefault();
}
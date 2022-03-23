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
    public Image timerFillAmount;

    protected float currentTime;
    protected float currentResetTime;
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
                if (pieces[i] == null) // 빈칸이라면
                {
                    print("비어있는 칸 : " + i);

                    piece.OnRullet();
                    piece.pieceIdx = i;
                    pieces[i] = piece;

                    break;
                }
            }
        }
        else
        {
            piece.OnRullet();
            piece.pieceIdx = pieces.Count;
            pieces.Add(piece);
        }

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

    public virtual void RollRullet(bool hasTimer = true)
    {
        if (isRoll) return;

        result = null;
        isRoll = true;
        isStop = false;

        timerFillAmount.fillAmount = 10;

        rollCor = StartCoroutine(Roll());

        if (hasTimer)
        {
            if(!isPaused)
                currentTime = GetTime();

            timeCor = StartCoroutine(Timer());
        }
    }

    private float GetTime()
    {
        return 10 - (rulletSpeed / 180f);
    }

    public virtual void PauseRullet()
    {
        if (!isRoll) return;    // 돌아가지 않는 상태거나
        if (isStop) return;     // 멈추고 있다면  리턴

        if (rollCor != null)
            StopCoroutine(rollCor);

        if(timeCor != null)
            StopCoroutine(timeCor);

        isPaused = true;
        isRoll = false;
    }

    public void StopRullet()
    {
        isStop = true;

        if (timeCor != null)
            StopCoroutine(timeCor);

        currentResetTime = currentTime;
        StartCoroutine(ResetTimer());
    }

    public void ReRoll()
    {
        rollSpeed = (600f + UnityEngine.Random.Range(0f, 50f)) * multiply;
        stopSpeed = UnityEngine.Random.Range(10f, 10.5f);
    }

    protected virtual IEnumerator Roll()
    {
        rollSpeed = (rulletSpeed + UnityEngine.Random.Range(0, 50)) * multiply;
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
        float counter = 0;

        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            counter = Mathf.Clamp(currentTime / GetTime(), 0f, 1f);

            timerFillAmount.fillAmount = counter;
            //255 -> 0
            //    237 -> 0
            timerFillAmount.color = new Color(1, counter, counter);

            yield return null;
        }

        isStop = true;
    }

    protected virtual IEnumerator ResetTimer()
    {
        float counter = 0;

        while (currentTime > 0)
        {
            currentTime -= currentResetTime / (0.25f / Time.deltaTime);
            counter = Mathf.Clamp(currentTime / GetTime(), 0f, 1f);
            timerFillAmount.fillAmount = counter;
            timerFillAmount.color = new Color(1, counter, counter);

            yield return null;
        }
    }

    protected virtual void RulletResult(Action<RulletPiece, int> onResult)
    {
        // 총 각도의 합
        float angleSum = 0f;
        // 크기 1당 각도
        float sizeAngle = 360f / maxSize;

        // -180 ~ 180 표현식을 0 ~ 360 표현식으로 변환하는 과정
        float rulletAngle = transform.eulerAngles.z;
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
        //fillTween = result.GetComponent<Image>().DOFillAmount(1f, 0.6f);
        result.transform.SetAsLastSibling();
        result.transform.DOScale(new Vector3(1.15f, 1.15f, 1f), 0.55f);
        result.Highlight();

        transform.DOShakePosition(0.2f, 15f, 50);
    }

    protected abstract void CastDefault();
}
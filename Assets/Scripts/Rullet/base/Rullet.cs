using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Rullet : MonoBehaviour
{
    [SerializeField] protected List<RulletPiece> pieces = new List<RulletPiece>();
    protected RulletPiece result;

    public Action<RulletPiece, int> onResult;
    public event Action onTimerEnd;

    protected int maxSize = 36;
    public bool IsRoll { get; private set; } = false;
    public bool IsStop { get; private set; } = false;

    protected float rollSpeed;
    protected float stopSpeed;
    protected float multiply = 1f;
    protected float rulletSpeed = 0f;
    public float RulletSpeed
    {
        get => rulletSpeed;
        set
        {
            rulletSpeed = value;

            rulletSpeed = Mathf.Clamp(rulletSpeed, 500f, 1500f);
            speedText.text = $"Speed : {rulletSpeed}";
        }
    }

    protected Coroutine rollCor;
    protected Coroutine timeCor;
    protected Tween fillTween;

    //public Transform pinTrans;
    public Text speedText;
    public Image timerFillAmount;
    public Gradient timerGradient;
    public List<ParticleSystem> particles = new List<ParticleSystem>();

    protected float currentTime;
    protected float currentResetTime;
    protected bool isPaused = false;

    protected WaitForSeconds oneSecWait = new WaitForSeconds(1f);

    protected virtual void Start()
    {
        timerFillAmount.color = timerGradient.Evaluate(0);

        ResetRulletSpeed();
        SetRullet();
        SetParticle(false);
    }

    public virtual void ResetRulletSize()
    {
        fillTween.Kill();

        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] == null)
            {
                continue;
            }

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
                    //print("����ִ� ĭ : " + i);

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
        // �� ũ���� ��
        int sizeSum = 0;
        // ũ�� 1�� ����
        float angle = -360f / maxSize;

        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i] != null)
            {
                pieces[i].transform
                            .DORotateQuaternion(Quaternion.AngleAxis(transform.eulerAngles.z + (sizeSum * angle), Vector3.forward), 0.35f);

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
            pieces[i].transform.rotation = Quaternion.AngleAxis(transform.eulerAngles.z + (sizeSum * angle), Vector3.forward);

            sizeSum += pieces[i].Size;
        }
    }

    public virtual void RollRullet(bool hasTimer = true)
    {
        if (IsRoll)
        {
            return;
        }

        result = null;
        IsRoll = true;
        IsStop = false;

        timerFillAmount.fillAmount = 10;

        rollCor = StartCoroutine(Roll());
        SetParticle(true);

        if (hasTimer)
        {
            if (!isPaused)
            {
                currentTime = GetTime();
            }

            timeCor = StartCoroutine(Timer());
        }
    }

    private float GetTime()
    {
        return 10 - (rulletSpeed / 180f);
    }

    private void SetParticle(bool enable)
    {
        for (int i = 0; i < particles.Count; i++)
        {
            if (enable)
            {
                particles[i].Play();
            }
            else
            {
                particles[i].Stop();
            }
        }
    }

    public virtual void PauseRullet()
    {
        if (!IsRoll)
        {
            return;    // ���ư��� �ʴ� ���°ų�
        }

        if (IsStop)
        {
            return;     // ���߰� �ִٸ�  ����
        }

        if (rollCor != null)
        {
            StopCoroutine(rollCor);
        }

        if (timeCor != null)
        {
            StopCoroutine(timeCor);
        }

        SetParticle(false);

        isPaused = true;
        IsRoll = false;
    }

    public void StopRullet()
    {
        IsStop = true;

        SetParticle(false);

        if (timeCor != null)
        {
            StopCoroutine(timeCor);
        }

        currentResetTime = currentTime;
        StartCoroutine(ResetTimer());
    }

    public void StopForceRullet()
    {
        if (rollCor != null)
        {
            StopCoroutine(rollCor);
        }

        if (timeCor != null)
        {
            StopCoroutine(timeCor);
        }

        SetParticle(false);

        IsStop = false;
        IsRoll = false;
        isPaused = false;
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

        speedText.text = $"Speed : {rulletSpeed}";

        while (Mathf.Abs(rollSpeed) > 1.5f)
        {
            yield return null;

            transform.Rotate(0f, 0f, rollSpeed * Time.deltaTime);

            if (IsStop)
            {
                rollSpeed = Mathf.Lerp(rollSpeed, 0f, Time.deltaTime * stopSpeed);
            }
        }

        IsStop = false;
        IsRoll = false;
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
            timerFillAmount.color = timerGradient.Evaluate(1 - counter);

            yield return null;
        }

        onTimerEnd?.Invoke();

        IsStop = true;
    }

    protected virtual IEnumerator ResetTimer()
    {
        float counter = 0;

        while (currentTime > 0)
        {
            currentTime -= currentResetTime / (0.25f / Time.deltaTime);
            counter = Mathf.Clamp(currentTime / GetTime(), 0f, 1f);
            timerFillAmount.fillAmount = counter;
            timerFillAmount.color = timerGradient.Evaluate(1 - counter);

            yield return null;
        }
    }

    protected virtual void RulletResult(Action<RulletPiece, int> onResult)
    {
        // �� ������ ��
        float angleSum = 0f;
        // ũ�� 1�� ����
        float sizeAngle = 360f / maxSize;

        // -180 ~ 180 ǥ������ 0 ~ 360 ǥ�������� ��ȯ�ϴ� ����
        float rulletAngle = transform.eulerAngles.z;
        ////print("�귿 ���� : " + rulletAngle);

        int pieceIdx = -1;

        for (int i = 0; i < pieces.Count; i++)
        {
            // ���� �ȿ� �ִٸ�
            if (rulletAngle <= angleSum + (pieces[i].Size * sizeAngle)
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
            //print($"{pieceIdx + 1}��° ��÷!");
            result = pieces[pieceIdx];
        }
        else
        {
            //print("�⺻ ��÷!");
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EffectObj : MonoBehaviour
{
    public AnimationCurve moveCurve;

    private SpriteRenderer sr;
    public SpriteRenderer Sr { 
        get 
        {
            if (sr == null)
                sr = GetComponent<SpriteRenderer>();

            return sr;
        } 
    }

    private TrailRenderer tr;
    private WaitForSeconds trailWait;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        tr = GetComponent<TrailRenderer>();

        float trailWaitTime = tr.time;
        trailWait = new WaitForSeconds(trailWaitTime);
    }

    public void SetSprite(Sprite sp)
    {
        Sr.color = Color.white;
        Sr.sprite = sp;
    }

    public void SetColorGradient(Gradient gradient)
    {
        tr.colorGradient = gradient;
    }

    public void SetScale(Vector3 scale)
    {
        transform.localScale = scale;
    }

    public void SetRandomSprite(List<Sprite> sprites)
    {
        if (sprites.Count > 0)
        {
            int randIdx = Random.Range(0, sprites.Count);
            Sr.sprite = sprites[randIdx];
        }
    }

    public void EndEffect()
    {
        Sr.color = Color.clear;
        transform.localScale = Vector3.one * 0.5f;

        StartCoroutine(EndWait());
    }

    public void EndEffectWithVisible()
    {
        StartCoroutine(EndWait());
    }

    private IEnumerator EndWait()
    {
        yield return trailWait;
        Sr.color = Color.white;

        gameObject.SetActive(false);
    }

    //                     끝점      끝났을 때 불리는 콜백 함수      배지어 타입 (기본 큐빅)        실행될 딜레이       실행될 스피드        선을 따라 회전할건지
    public void Play(Vector3 target, Action onEndEffect, BezierType type = BezierType.Cubic, float delay = 0f, float playSpeed = 1.6f, bool isRotate = false)
    {
        if (isRotate)
        {
            Vector3 dir = target - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        StartCoroutine(PlayEffect(target, onEndEffect, type, delay, playSpeed, isRotate));
    }

    private IEnumerator PlayEffect(Vector3 target, Action onEndEffect, BezierType type, float delay, float playSpeed, bool isRotate)
    {
        Vector3 start = transform.position;

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 p0 = start + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 2f;

        angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 p1 = target + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 2f;

        Vector3 prevPoint = start;
        Vector3 dir = Vector3.zero;

        transform.rotation = Quaternion.identity;

        if (delay > 0)
            yield return new WaitForSeconds(delay);

        float t = 0f;

        while (t < 1f)
        {
            yield return null;

            switch (type)
            {
                case BezierType.Quadratic:
                    transform.position = QuadraticBezierPoint(moveCurve.Evaluate(t), start, p0, target);

                    break;

                case BezierType.Cubic:
                    transform.position = CubicBezierPoint(moveCurve.Evaluate(t), start, p0, p1, target);

                    break;

                case BezierType.Linear:
                    transform.position = LinearBezierPoint(moveCurve.Evaluate(t), start, target);

                    break;
            }

            if (isRotate)
            {
                dir = transform.position - prevPoint;
                angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                prevPoint = transform.position;
            }

            t += Time.deltaTime * playSpeed;
        }

        onEndEffect?.Invoke();

        yield return null;
    }

    private Vector3 CubicBezierPoint(float t, Vector3 start, Vector3 p1, Vector3 p2, Vector3 end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * start;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * end;

        return p;
    }

    private Vector3 QuadraticBezierPoint(float t, Vector3 start, Vector3 p1, Vector3 end)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * start;
        p += 2 * u * t * p1;
        p += tt * end;

        return p;
    }

    private Vector3 LinearBezierPoint(float t, Vector3 start, Vector3 end)
    {
        return start + t * (end - start);
    }
}

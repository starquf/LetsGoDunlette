using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EffectObj : MonoBehaviour
{
    public List<Sprite> randomSprites = new List<Sprite>();

    public void Start()
    {
        if (randomSprites.Count > 0)
        {
            int randIdx = Random.Range(0, randomSprites.Count);
            GetComponent<SpriteRenderer>().sprite = randomSprites[randIdx];
        }
    }

    public void SetSprite(Sprite sp)
    {
        GetComponent<SpriteRenderer>().sprite = sp;
    }

    public void Play(Vector3 target, Action onEndEffect, BezierType type = BezierType.Cubic, float delay = 0f)
    {
        StartCoroutine(PlayEffect(target, onEndEffect, type, delay));
    }

    private IEnumerator PlayEffect(Vector3 target, Action onEndEffect, BezierType type = BezierType.Cubic, float delay = 0f)
    {
        if (delay > 0)
            yield return new WaitForSeconds(delay);

        Vector3 start = transform.position;

        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 p0 = start + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 2f;

        angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 p1 = target + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 2f;

        float t = 0f;
        float speedPlus = 0f;

        while (t < 1f)
        {
            yield return null;

            switch (type)
            {
                case BezierType.Quadratic:
                    transform.position = QuadraticBezierPoint(t, start, p0, target);

                    break;

                case BezierType.Cubic:
                    transform.position = CubicBezierPoint(t, start, p0, p1, target);

                    break;
            }

            t += Time.deltaTime * speedPlus;
            speedPlus += 0.02f;
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
}

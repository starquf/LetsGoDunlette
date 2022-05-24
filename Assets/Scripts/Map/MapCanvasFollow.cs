using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCanvasFollow : MonoBehaviour
{
    [HideInInspector] public Transform targetTrm;

    public float followSpeed;

    private RectTransform rectTrm;
    private float defaultZoomScale;
    private Vector2 defaultAnchorPos;
    private Sequence zoomTween;
    private Coroutine followCoroutine = null;

    private MapManager mapManager = null;


    private void Awake()
    {
        rectTrm = GetComponent<RectTransform>();
        defaultZoomScale = rectTrm.localScale.x;
        defaultAnchorPos = rectTrm.anchoredPosition;
    }

    private void Start()
    {
        mapManager = GameManager.Instance.mapManager;
    }

    public void FollowSkip()
    {
        Vector2 position = transform.position - (targetTrm == null ? Vector3.zero : targetTrm.position);
        transform.position = position;
    }

    public void Follow(float speedScale = 1, Action onEndAnim= null)
    {
        if(followCoroutine != null)
            StopCoroutine(followCoroutine);
        followCoroutine = StartCoroutine(FollowAnim(speedScale, onEndAnim));
    }

    private IEnumerator FollowAnim(float speedScale, Action onEndAnim)
    {
        Vector2 position = transform.position - (targetTrm == null ? Vector3.zero : targetTrm.position);
        float dist = Vector2.Distance(position, transform.position);
        while (dist > 0.01)
        {
            dist = Vector2.Distance(position, transform.position);

            transform.position = Vector2.Lerp(transform.position, position, Time.deltaTime * followSpeed * speedScale);
            yield return null;
        }
        mapManager.SetDefaultMapPos();
        onEndAnim?.Invoke();
    }

    public void Zoom(float zoomScale, bool skip = false, float time = 0.5f, Ease ease = Ease.Unset, Action onComplete = null)
    {
        //playerFollowCam.Follow = orthographicSize >= 5 ? playerUpTrm : playerTrm;
        //MoveBossCloud(orthographicSize, time, ease, skip);

        Vector3 targetScale = new Vector3(defaultZoomScale * zoomScale, defaultZoomScale * zoomScale, 1);
        Vector2 targetAmchorPos = new Vector2(rectTrm.anchoredPosition.x * (zoomScale==1?0:zoomScale) / 2 , rectTrm.anchoredPosition.y * (zoomScale == 1 ? 0 : zoomScale) / 2);

        if (!skip)
        {
            zoomTween.Kill();
            zoomTween = DOTween.Sequence()
                .Append(rectTrm.DOScale(targetScale, time).SetEase(ease))
                .Join(rectTrm.DOAnchorPos(targetAmchorPos, time).SetEase(ease))
                .OnComplete(()=> {
                    mapManager.SetDefaultMapPos();
                    onComplete?.Invoke();
                });
        }
        else
        {
            rectTrm.localScale = targetScale;
            rectTrm.anchoredPosition = targetAmchorPos;
            mapManager.SetDefaultMapPos();
            onComplete?.Invoke();
        }
    }
}
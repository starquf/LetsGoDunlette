using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleTargetSelectHandler : MonoBehaviour
{
    public Transform detectTrans;
    public InventoryInfoHandler invenInfoHandler;
    public Image arrowImg;

    private LineRenderer lr;

    private bool canControl = true;

    private bool isSelect = false;
    private bool isDrag = false;

    private Camera mainCam;

    private Vector3 touchPos;
    public float detectDistance = 5f;

    private Action<EnemyHealth> onEndSelect;

    private LayerMask isEnemy;

    public SkipUIPanelHandler skipUI;
    private Sequence dragSeq;

    private void Start()
    {
        lr = detectTrans.GetComponent<LineRenderer>();

        invenInfoHandler.invenBtn.onClick.AddListener(() => canControl = false);
        invenInfoHandler.closeBtn.onClick.AddListener(() => canControl = true);
        invenInfoHandler.closeImgBtn.onClick.AddListener(() => canControl = true);

        mainCam = Camera.main;
        isEnemy = LayerMask.GetMask("Enemy");

        arrowImg.enabled = false;
    }

    private void Update()
    {
        if (!canControl) return;

        CheckDrag();
    }

    private void CheckDrag()
    {
        if (!isSelect || isDrag) return;

        if (Input.GetMouseButtonDown(0))
        {
            dragSeq.Kill();
            skipUI.SetPanel(false);

            touchPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            touchPos.z = 0f;

            Vector3 detectPos = detectTrans.position;
            detectPos.z = 0f;

            Vector3 dir = touchPos - detectPos;

            // 스킬을 잡았다면
            if (dir.sqrMagnitude < detectDistance * detectDistance)
            {

                lr.SetPosition(0, detectPos);
                lr.SetPosition(1, detectPos);

                arrowImg.enabled = true;
                arrowImg.transform.position = detectPos;

                StartCoroutine(OnDrag());
            }
        }
    }

    public void SelectTarget(Action<EnemyHealth> onEndSelect)
    {
        isSelect = true;

        ShowDrag();

        this.onEndSelect = onEndSelect;
    }

    private IEnumerator OnDrag()
    {
        isDrag = true;

        while (true)
        {
            yield return null;

            touchPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            touchPos.z = 0f;

            // 적이 선택되었다면
            if (GetPositionEnemy(touchPos, out EnemyHealth enemy))
            {
                // 선 그려주는 작업
                lr.SetPosition(1, enemy.transform.position);
                lr.endColor = Color.red;
                arrowImg.color = Color.red;
            }
            else
            {
                lr.endColor = Color.white;
                arrowImg.color = Color.white;
                // 선 그려주는 작업
                lr.SetPosition(1, touchPos);
            }

            SetArrowPos();

            // 손을 때면
            if (Input.GetMouseButtonUp(0))
            {
                // 적이 선택되었다면
                if (enemy != null)
                {
                    SetTarget(enemy);
                }
                else
                {
                    dragSeq.Kill();
                    skipUI.SetPanel(false);

                    ShowDrag();
                }

                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, Vector3.zero);

                isDrag = false;
                arrowImg.enabled = false;

                break;
            }
        }
    }

    private void ShowDrag()
    {
        dragSeq = DOTween.Sequence()
                        .AppendInterval(1.2f)
                        .AppendCallback(() =>
                        {
                            skipUI.ShowDragUI();
                        });
    }

    private void SetArrowPos()
    {
        Vector2 dir = lr.GetPosition(1) - lr.GetPosition(0);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        arrowImg.transform.position = lr.GetPosition(1);
        arrowImg.transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
    }

    private void SetTarget(EnemyHealth enemy)
    {
        onEndSelect(enemy);

        isSelect = false;
    }

    private bool GetPositionEnemy(Vector3 pos, out EnemyHealth enemy)
    {
        enemy = null;

        Collider2D coll = Physics2D.OverlapPoint(pos, isEnemy);

        // 적이 있다면
        if (coll != null)
        {
            enemy = coll.GetComponent<EnemyHealth>();
            return true;
        }

        return false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTargetSelectHandler : MonoBehaviour
{
    public Transform detectTrans;
    public InventoryInfoHandler invenInfoHandler;

    private LineRenderer lr;

    private bool canControl = true;

    private bool isSelect = false;
    private bool isDrag = false;

    private Camera mainCam;

    private Vector3 touchPos;
    public float detectDistance = 5f;

    private Action<EnemyHealth> onEndSelect;

    private LayerMask isEnemy;

    private void Start()
    {
        lr = detectTrans.GetComponent<LineRenderer>();

        invenInfoHandler.invenBtn.onClick.AddListener(() => canControl = false);
        invenInfoHandler.closeBtn.onClick.AddListener(() => canControl = true);

        mainCam = Camera.main;
        isEnemy = LayerMask.GetMask("Enemy");
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

                StartCoroutine(OnDrag());
            }
        }
    }

    public void SelectTarget(Action<EnemyHealth> onEndSelect)
    {
        isSelect = true;

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

            // 선 그려주는 작업
            lr.SetPosition(1, touchPos);

            // 손을 때면
            if (Input.GetMouseButtonUp(0))
            {
                DetectEnemy();

                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, Vector3.zero);

                isDrag = false;

                break;
            }
        }
    }

    private void DetectEnemy()
    {
        // 적이 있다면
        if (GetPositionEnemy(touchPos, out EnemyHealth enemy))
        {
            onEndSelect(enemy);

            isSelect = false;
        }
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

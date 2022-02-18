using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShakeHandler : MonoBehaviour
{
    public RectTransform backCanvasUI;

    private void Awake()
    {
        GameManager.Instance.shakeHandler = this;
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    // BackCvsUI 흔들때 부르는 함수
    public void ShakeBackCvsUI(float intensity, float time)
    {
        backCanvasUI.DOKill();
        backCanvasUI.localPosition = Vector3.zero;
        backCanvasUI.DOShakePosition(time, intensity*100 , 30);
        //backCanvasUI.DOShakePosition(time, intensity*100, 50);
    }
}

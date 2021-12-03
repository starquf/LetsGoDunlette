using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraHandler : MonoBehaviour
{
    private CinemachineVirtualCamera virtualCam;
    private CinemachineBasicMultiChannelPerlin camPerlin;

    private float shakeDur = 0f;

    private void Awake()
    {
        GameManager.Instance.cameraHandler = this;
    }

    private void Start()
    {
        virtualCam = GetComponent<CinemachineVirtualCamera>();
        camPerlin = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    private void Update()
    {
        ShakeTimer();
    }

    // ī�޶� ����ŷ Ÿ�̸� �Լ�
    private void ShakeTimer()
    {
        if (shakeDur > 0)
        {
            shakeDur -= Time.deltaTime;

            if (shakeDur <= 0)
            {
                camPerlin.m_AmplitudeGain = 0;
            }
        }
    }

    // ī�޶� ��鶧 �θ��� �Լ�
    public void ShakeCamera(float intensity, float time)
    {
        camPerlin.m_AmplitudeGain = intensity;
        shakeDur = time;
    }
}

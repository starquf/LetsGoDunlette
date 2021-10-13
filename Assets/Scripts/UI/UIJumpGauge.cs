using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIJumpGauge : MonoBehaviour
{
    public PlayerInput playerInput;
    public JumpForce jumpForce;
    public Image gaugeFillImage;

    private void Update()
    {
        if (playerInput.Jump_Pressing)
        {
            gaugeFillImage.fillAmount = jumpForce.JumpForce01;
        }
        else
        {
            gaugeFillImage.fillAmount = 0;
        }
    }
}

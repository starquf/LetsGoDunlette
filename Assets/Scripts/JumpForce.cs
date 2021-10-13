using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpForce : MonoBehaviour
{
    public PlayerInput playerInput;

    public float JumpForce01 => Mathf.Clamp01(currentJumpForce / maxJumpForce);
    public float jumpForce01Temp;

    public float maxJumpForce; //최대 점프량
    private float currentJumpForce = 0; //현제 점프량

    public float jumpForcePerSec = 1f; //1초당 쌓일 

    private float tempJumpForce;

    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
    }

    private void Update()
    {
        print(playerInput.Jump_Pressing);
        if (playerInput.Jump_Pressing)
        {
            currentJumpForce += Time.deltaTime * jumpForcePerSec;
            currentJumpForce = Mathf.Clamp(currentJumpForce, 0, maxJumpForce);
            jumpForce01Temp = JumpForce01;
        }
        else
        {
            currentJumpForce = 0;
        }
    }
}

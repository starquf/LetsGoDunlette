using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : MonoBehaviour
{
    private PlayerInput playerInput = null;
    private JumpForce jumpForceCS;
    private Rigidbody2D rb = null;

    [Header("점프 관련")]
    [SerializeField] private float jumpForce = 13f;
    [SerializeField] private float fallMult = 2.5f;
    [SerializeField] private float lowJumpMult = 5f;

    private void Start()
    {
        playerInput = FindObjectOfType<PlayerInput>();
        jumpForceCS = FindObjectOfType<JumpForce>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Jump();
    }

    private void Jump()
    {
        // 점프 키를 누를 시
        if (playerInput.Jump_Up)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce * jumpForceCS.jumpForce01Temp, ForceMode2D.Impulse);
        }

        // 아직 붕 뜨고 있는 상태
        if (rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMult - 1) * Time.deltaTime;
        }

        // 낙하 시에 && 점프를 누르지 않을 시에
        else if (rb.velocity.y > 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMult - 1) * Time.deltaTime;
        }
    }
}
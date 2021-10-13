using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool Jump_Down { get; private set; }
    public bool Jump_Pressing { get; private set; }
    public bool Jump_Up { get; private set; }

    private void Update()
    {
        Jump_Down = Input.GetKeyDown(KeyCode.Space);
        Jump_Pressing = Input.GetKey(KeyCode.Space);
        Jump_Up = Input.GetKeyUp(KeyCode.Space);
    }
}
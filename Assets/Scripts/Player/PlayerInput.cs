using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool JumpPressed { get; private set; }
    public bool JumpReleased { get; private set; }
    public bool CrouchHeld { get; private set; }
    public bool JumpHeld { get; private set; }

    private void Update()
    {
        JumpPressed = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow);
        JumpReleased = Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.UpArrow);
        JumpHeld = Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow);
        CrouchHeld = Input.GetKey(KeyCode.DownArrow);
    }
}

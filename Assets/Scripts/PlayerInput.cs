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
        JumpPressed = Input.GetKeyDown(KeyCode.W);
        JumpReleased = Input.GetKeyUp(KeyCode.W);
        JumpHeld = Input.GetKey(KeyCode.W);
        CrouchHeld = Input.GetKey(KeyCode.S);
    }
}

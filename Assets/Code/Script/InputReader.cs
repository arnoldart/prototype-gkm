using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, Controls.IGameplayActions
{
    public Vector2 MovementValue { get; private set; }

    public event Action JumpEvent;

    private Controls controls;

    private void Start()
    {
        controls = new Controls();
        controls.Gameplay.SetCallbacks(this);
        
        controls.Gameplay.Enable();
    }

    private void Update()
    {
        
    }

    private void OnDestroy()
    {
        controls.Gameplay.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed) {return;}
        JumpEvent?.Invoke();
    }
}

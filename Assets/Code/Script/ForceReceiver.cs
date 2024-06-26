using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController Controller;

    private float verticalVelocity;

    public Vector3 movement => Vector3.up * verticalVelocity;

    private void Update()
    {
        if (verticalVelocity <= 0f && Controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }
}

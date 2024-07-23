using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    private bool shouldFade;
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private const float CrossFadeDuration = .1f;
    private const float AnimatorDampTime = .1f;
    
    public PlayerFreeLookState(PlayerStateMachine stateMachine, bool shouldFade = true) : base(stateMachine)
    {
        this.shouldFade = shouldFade;
    }

    public override void Enter()
    {
        // stateMachine.InputReader.RunningEvent += OnRunning;
        stateMachine.InputReader.JumpEvent += OnJump;
        stateMachine.InputReader.EquipWeapon += OnEquipWeapon;

        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        
        // if (shouldFade)
        // {
        //     stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
        // }
        // else
        // {
        //     stateMachine.Animator.Play(FreeLookBlendTreeHash);
        // }

    }

    public override void Tick(float deltaTime)
    {
        Vector3 movement = CalculateMove();
        stateMachine.transform.Translate(movement * deltaTime);
        Move(movement * stateMachine.FreeLookMovementSpeed, deltaTime);
        
        if (stateMachine.InputReader.MovementValue == Vector2.zero)
        {
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 0, AnimatorDampTime, deltaTime);
            stateMachine.IsRunning = false;
            return;
        }

        if(stateMachine.IsRunning)
        {
            Move(movement * stateMachine.RunningMovementSpeed, deltaTime);
            stateMachine.Animator.SetFloat(FreeLookSpeedHash, 2, AnimatorDampTime, deltaTime);
        }

        if (!stateMachine.Controller.isGrounded)
        {
            stateMachine.SwitchState(new PlayerFallingState(stateMachine));
            return;
        }
        
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 1, AnimatorDampTime, deltaTime);

        FaceMovementDirection(movement, deltaTime);
    }

    public override void Exit()
    {
        // stateMachine.InputReader.RunningEvent -= OnRunning;
        stateMachine.InputReader.JumpEvent -= OnJump;
        stateMachine.InputReader.EquipWeapon -= OnEquipWeapon;
    }

    private Vector3 CalculateMove()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.InputReader.MovementValue.y + right * stateMachine.InputReader.MovementValue.x;
    }
    
    private void FaceMovementDirection(Vector3 movement, float deltatime)
    {
        stateMachine.transform.rotation = Quaternion.Lerp( stateMachine.transform.rotation, Quaternion.LookRotation(movement), deltatime * stateMachine.RotationDamping);
    }

    private void OnJump()
    {
        stateMachine.SwitchState(new PlayerJumpState(stateMachine));
    }

    private void OnEquipWeapon()
    {
        stateMachine.isWeaponEquip = !stateMachine.isWeaponEquip;
        
        Debug.Log(stateMachine.isWeaponEquip);

        if (stateMachine.isWeaponEquip)
        {
            EquipWeapon();
        }
        else
        {
            UnEquipWeapon();
        }
    }

    void EquipWeapon()
    {
        if (stateMachine.weaponUnequipPosition.childCount > 0)
        {
            GameObject weapon = stateMachine.weaponUnequipPosition.GetChild(0).gameObject;
            weapon.transform.SetParent(stateMachine.weaponEquipPosition);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.Euler(0, 0, -90);
            SetGlobalScale(weapon.transform, new Vector3(0.15f, 0.15f, 0.15f));
            stateMachine.isWeaponEquip = true;
        }
    }

    void UnEquipWeapon()
    {
        if (stateMachine.weaponEquipPosition.childCount > 0)
        {
            GameObject weapon = stateMachine.weaponEquipPosition.GetChild(0).gameObject;
            weapon.transform.SetParent(stateMachine.weaponUnequipPosition);
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = quaternion.identity;
            SetGlobalScale(weapon.transform, new Vector3(0.15f, 0.15f, 0.15f));
            stateMachine.isWeaponEquip = false;
        }
    }
    
    private void SetGlobalScale(Transform transform, Vector3 globalScale)
    {
        transform.localScale = Vector3.one;
        transform.localScale = new Vector3(
            globalScale.x / transform.lossyScale.x,
            globalScale.y / transform.lossyScale.y,
            globalScale.z / transform.lossyScale.z
        );
    }

    // private void OnRunning() 
    // {
    //     stateMachine.SwitchState(new PlayerRunningState(stateMachine));
    // }
}

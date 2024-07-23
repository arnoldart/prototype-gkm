using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunningState : PlayerBaseState
{
    private readonly int FreeLookBlendTreeHash = Animator.StringToHash("FreeLookBlendTree");
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private const float CrossFadeDuration = .1f;
    private const float AnimatorDampTime = .1f;

    public PlayerRunningState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.Animator.CrossFadeInFixedTime(FreeLookBlendTreeHash, CrossFadeDuration);
    }

    public override void Tick(float deltaTime)
    {
        
        stateMachine.Animator.SetFloat(FreeLookSpeedHash, 2, AnimatorDampTime, deltaTime);
        Debug.Log("Running");

        if(stateMachine.IsRunning)
        {
            Debug.Log("Idle");
            stateMachine.SwitchState(new PlayerFreeLookState(stateMachine));
        }
    }

    public override void Exit()
    {
        
    }
}

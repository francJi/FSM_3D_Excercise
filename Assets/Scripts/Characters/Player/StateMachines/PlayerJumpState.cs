using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.JumpForce = stateMachine.Player.Data.AirData.JumpForce; //jumpForce를 airData에 있는 jumpForce로 등록
        stateMachine.Player.ForceReceiver.Jump(stateMachine.JumpForce); // ForceReceiver에 등록된 jumpForce 값 제공

        base.Enter();

        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // 공중에서 땅으로 떨어지는 시점이 될 때 (y가 0보다 작아질 때) 떨어지는 상태 (fallState) 적용
        if (stateMachine.Player.Controller.velocity.y <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}

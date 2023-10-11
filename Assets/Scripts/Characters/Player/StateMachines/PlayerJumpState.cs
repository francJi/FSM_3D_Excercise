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
        stateMachine.JumpForce = stateMachine.Player.Data.AirData.JumpForce; //jumpForce�� airData�� �ִ� jumpForce�� ���
        stateMachine.Player.ForceReceiver.Jump(stateMachine.JumpForce); // ForceReceiver�� ��ϵ� jumpForce �� ����

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

        // ���߿��� ������ �������� ������ �� �� (y�� 0���� �۾��� ��) �������� ���� (fallState) ����
        if (stateMachine.Player.Controller.velocity.y <= 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}

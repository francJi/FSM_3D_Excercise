using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    // �̵��� ���� ó���� ���� BaseState���� �ϹǷ�, ���⼭�� �ִϸ��̼� ���븸 ���ָ� ��.
    public PlayerWalkState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter(); // ground�� ��ӹ����Ƿ�, ground�� Enter ���� -> groundParameterHash�� �ִϸ��̼� ����
        StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash); // ���������� ground�� walk�� 2���� Hash���� �ش��ϴ� bool�� ����
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);
    }

    protected override void OnRunStarted(InputAction.CallbackContext context)
    {
        base.OnRunStarted(context); 
        stateMachine.ChangeState(stateMachine.RunState); // Run Ű�� �Է¹�����, RunState�� ��ȯ
    }
}

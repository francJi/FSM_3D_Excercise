using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    // �̵��� ���� ó���� ���� BaseState���� �ϹǷ�, ���⼭�� �ִϸ��̼� ���븸 ���ָ� ��.
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        base.Enter(); // ground�� ��ӹ����Ƿ�, ground�� Enter ���� -> groundParameterHash�� �ִϸ��̼� ����
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash); // ���������� ground�� walk�� 2���� Hash���� �ش��ϴ� bool�� ����
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }
}

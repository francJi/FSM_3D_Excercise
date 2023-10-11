using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ߺ��� �����ϱ� ���� �ٷ� IState �������̽��� ��ӹ޴� ���� �ƴ϶�, ���ο� ������Ʈ base state�� ��ӹް� �� ����
public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    
    public override void Enter()
    {
        // Idle ���¿����� ������ ���� �ű� ������ �̵��ӵ��� �ǹ̰� ����.
        stateMachine.MovementSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.IdleParameterHash);
    }

    public override void HandleInput()
    {
        
    }

    public override void PhysicsUpdate()
    {
        
    }

    public override void Update()
    {
        base.Update();
    }
}

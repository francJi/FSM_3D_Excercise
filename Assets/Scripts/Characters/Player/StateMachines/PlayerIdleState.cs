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
        //base state�� �ִ� HandleInput�� ����ϴµ� . ���Ⱑ ����ִµ� �������̵带 �޾Ƽ� ������ ���� ����.
        // �������̵�� �ʿ��� ���� ���
        base.HandleInput();
    }


    public override void Update()
    {
        base.Update();

        if (stateMachine.MovementInput != Vector2.zero) // �̵������� Input�� �Է¹޾Ҵٸ�
        {
            OnMove(); // �̵�ó�� // OnMove���� State�� ��ȯ��Ŵ.
            return;
        }
    }
}

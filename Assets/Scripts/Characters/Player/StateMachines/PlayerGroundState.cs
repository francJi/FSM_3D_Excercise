using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // ground�� ���� hash������ bool���� ����
        // ground�� �����ִ� state���� ���� groundstate��� bool���� �����ִ� ���·� ������ ����
        StartAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Exit() 
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.GroundParameterHash);
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        // ���� ���°� ���� �ƴ� ��(isGrounded false & y�� �ӵ��� �߷��� �ӵ����� Ŭ ��)
        // => FallState�� ��ȯ�ϴ� ��� �߰�
        if (!stateMachine.Player.Controller.isGrounded
            && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // �Է�ó���� ������ �ʾҴٸ�, return
        if (stateMachine.MovementInput == Vector2.zero)
        {
            return;
        }

        // �Է�ó���� ���Դٰ� ��Ұ� �� ��Ȳ (ex. �̵� Ű�� ������ ��)
        // ground�� �ƴ� �ٸ� state�� ���� ���� �ٸ� ������ �������.
        stateMachine.ChangeState(stateMachine.IdleState);

        base.OnMovementCanceled(context);
    }

    protected virtual void OnMove()
    {
        stateMachine.ChangeState(stateMachine.WalkState);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.JumpState); // ������ �ϸ�, ���� ������Ʈ�� �ѱ�. ����, ���� ������Ʈ���� �˾Ƽ� ó���� ����.
    }
}

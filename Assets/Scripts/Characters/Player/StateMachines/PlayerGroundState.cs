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
        // ground에 대한 hash값으로 bool값을 켜줌
        // ground를 속해있는 state들은 전부 groundstate라는 bool값이 켜져있는 상태로 동작할 것임
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
        // 현재 상태가 땅이 아닐 때(isGrounded false & y축 속도가 중력의 속도보다 클 때)
        // => FallState로 전환하는 기능 추가
        if (!stateMachine.Player.Controller.isGrounded
            && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        // 입력처리가 들어오지 않았다면, return
        if (stateMachine.MovementInput == Vector2.zero)
        {
            return;
        }

        // 입력처리가 들어왔다가 취소가 된 상황 (ex. 이동 키가 떼졌을 때)
        // ground가 아닌 다른 state에 있을 때는 다른 동작을 지녀야함.
        stateMachine.ChangeState(stateMachine.IdleState);

        base.OnMovementCanceled(context);
    }

    protected virtual void OnMove()
    {
        stateMachine.ChangeState(stateMachine.WalkState);
    }

    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        stateMachine.ChangeState(stateMachine.JumpState); // 점프를 하면, 점프 스테이트로 넘김. 이후, 점프 스테이트에서 알아서 처리할 것임.
    }
}

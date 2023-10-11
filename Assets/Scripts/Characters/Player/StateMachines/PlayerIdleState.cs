using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 중복을 방지하기 위해 바로 IState 인터페이스를 상속받는 것이 아니라, 새로운 스테이트 base state를 상속받게 할 예정
public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    
    public override void Enter()
    {
        // Idle 상태에서는 가만히 있을 거기 때문에 이동속도가 의미가 없음.
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

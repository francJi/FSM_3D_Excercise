using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}

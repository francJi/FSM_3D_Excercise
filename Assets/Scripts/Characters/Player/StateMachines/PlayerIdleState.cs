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
        //base state에 있는 HandleInput을 써야하는데 . 여기가 비어있는데 오버라이드를 받아서 동작을 하지 않음.
        // 오버라이드는 필요할 때만 사용
        base.HandleInput();
    }


    public override void Update()
    {
        base.Update();

        if (stateMachine.MovementInput != Vector2.zero) // 이동에관한 Input을 입력받았다면
        {
            OnMove(); // 이동처리 // OnMove에서 State를 변환시킴.
            return;
        }
    }
}

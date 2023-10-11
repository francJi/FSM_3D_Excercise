using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerGroundState
{
    // 이동에 관한 처리는 전부 BaseState에서 하므로, 여기서는 애니메이션 적용만 해주면 됨.
    public PlayerRunState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        base.Enter(); // ground를 상속받으므로, ground의 Enter 실행 -> groundParameterHash의 애니메이션 실행
        StartAnimation(stateMachine.Player.AnimationData.RunParameterHash); // 최종적으로 ground와 walk의 2가지 Hash값에 해당하는 bool이 켜짐
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.RunParameterHash);
    }
}

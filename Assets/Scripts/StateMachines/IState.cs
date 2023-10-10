using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter(); // 해당 스테이트로 들어왔을 때
    public void Exit();  // 해당 스테이트를 나갈 때,
    public void HandleInput(); // 해당 스테이트에서 입력 처리를 할 때,
    public void Update(); // 업데이트
    public void PhysicsUpdate(); // 물리적 업데이트
}

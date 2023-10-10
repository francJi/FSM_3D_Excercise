using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 객체화를 할 수 없게 추상클래스로 구현
public abstract class StateMachine
{
    protected IState currentState; // 현재 state 정보

    public void ChangeState(IState newstate)
    {
        currentState?.Exit(); // 현재 상황이 있따면 exit
        currentState = newstate; // 새로운 상황을 적용
        currentState?.Enter(); 
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        currentState?.Update();
    }

    public void PhysicsUpdate() 
    {
        currentState?.PhysicsUpdate();
    }
}

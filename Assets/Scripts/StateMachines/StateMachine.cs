using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��üȭ�� �� �� ���� �߻�Ŭ������ ����
public abstract class StateMachine
{
    protected IState currentState; // ���� state ����

    public void ChangeState(IState newstate)
    {
        currentState?.Exit(); // ���� ��Ȳ�� �ֵ��� exit
        currentState = newstate; // ���ο� ��Ȳ�� ����
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void Enter(); // �ش� ������Ʈ�� ������ ��
    public void Exit();  // �ش� ������Ʈ�� ���� ��,
    public void HandleInput(); // �ش� ������Ʈ���� �Է� ó���� �� ��,
    public void Update(); // ������Ʈ
    public void PhysicsUpdate(); // ������ ������Ʈ
}

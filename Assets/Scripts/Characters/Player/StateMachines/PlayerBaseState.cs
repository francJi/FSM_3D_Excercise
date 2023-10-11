using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// ������ �����鼭 , �ʿ��� �͵�?�� ������� ����
// �� state���� PlayerBaseState�� ��ӹ����鼭, ���ϴ� �޼��忡 ���� �������̵带 �� ����
public class PlayerBaseState : IState
{
    // �⺻������ ��� state�� state machine�� �������� �ϰ� ��.

    protected PlayerStateMachine stateMachine;
    protected readonly PlayerGroundData groundData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Player.Data.GroundData;
    }

    public virtual void Enter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void Exit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void HandleInput()
    {
        ReadMovementInput();
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    public virtual void Update()
    {
        Move();
    }

    // input action �� ���� callback
    // �� ��Ȳ���� state�� ���� ��, �������� ���Ǵ� �Ϳ� ���ؼ� callback�� �޾Ƶ�.
    // �� state�� ���� ��, �ʿ��� �޼��带 �����ϰų� ������ �� �ְԵ�.
    protected virtual void AddInputActionsCallbacks()
    {

    }

    protected virtual void RemoveInputActionsCallbacks()
    {

    }

    private void ReadMovementInput()
    {
        stateMachine.MovementInput = stateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
    }

    // ���� �̵� ó��
    private void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Rotate(movementDirection);

        Move(movementDirection);
    }

    private Vector3 GetMovementDirection()
    {
        Vector3 forward = stateMachine.MainCameraTransform.forward;
        Vector3 right = stateMachine.MainCameraTransform.right;

        // y���� �����ؾ� ���ٴ��� �Ⱥ�
        forward.y = 0;
        right.y = 0;

        // Normalize vs Normalized
        forward.Normalize();
        right.Normalize();

        // �̵��ؾ��ϴ� ���� (�հ� ����)�� ���� �Է��� �̵������� ���� ���ؼ� ó��
        return forward * stateMachine.MovementInput.y + right * stateMachine.MovementInput.x;
    }


    private void Move(Vector3 movementDirection)
    {
        float movementSpeed = GetMovementSpeed();
        stateMachine.Player.Controller.Move(
            (movementDirection * movementSpeed) * Time.deltaTime
            );
    }

    private float GetMovementSpeed()
    {
        float movementSpeed = stateMachine.MovementSpeed * stateMachine.MovementSpeedModifier;
        return movementSpeed;
    }

    private void Rotate(Vector3 movementDirection)
    {
        if (movementDirection != Vector3.zero) 
        {
            Transform playerTransform = stateMachine.Player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(movementDirection);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.RotationDamping * Time.deltaTime);
        }
    }

    // State ���� Animation ó���� �ؾ���.
    // SetBool�� animation�� bool���� ó��

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash) 
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }
}

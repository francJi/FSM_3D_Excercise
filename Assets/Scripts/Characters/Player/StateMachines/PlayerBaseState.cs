using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// 구성해 나가면서 , 필요한 것들?을 집어넣을 예정
// 각 state들이 PlayerBaseState를 상속받으면서, 원하는 메서드에 대해 오버라이드를 할 예정
public class PlayerBaseState : IState
{
    // 기본적으로 모든 state는 state machine과 역참조를 하게 됨.

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

    // input action 에 대한 callback
    // 각 상황별로 state가 들어갔을 때, 공용으로 사용되는 것에 대해서 callback을 달아둠.
    // 그 state에 들어갔을 때, 필요한 메서드를 연결하거나 해지할 수 있게됨.
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

    // 실제 이동 처리
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

        // y값을 제거해야 땅바닥을 안봄
        forward.y = 0;
        right.y = 0;

        // Normalize vs Normalized
        forward.Normalize();
        right.Normalize();

        // 이동해야하는 벡터 (앞과 우측)에 각각 입력한 이동방향의 값을 곱해서 처리
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

    // State 마다 Animation 처리를 해야함.
    // SetBool로 animation의 bool값만 처리

    protected void StartAnimation(int animationHash)
    {
        stateMachine.Player.Animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash) 
    {
        stateMachine.Player.Animator.SetBool(animationHash, false);
    }
}

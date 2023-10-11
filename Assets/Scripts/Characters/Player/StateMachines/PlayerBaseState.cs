using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    // 키 입력 처리
    protected virtual void AddInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.Player.Input;
        input.PlayerActions.Movement.canceled += OnMovementCanceled;
        input.PlayerActions.Run.started += OnRunStarted;
        input.PlayerActions.Jump.started += OnJumpStarted;
        // 지금은 공격이던 점프건 이동이건 모두 공통되기 때문에 한 곳에 몰아두었지만, 
        // 추후에 프로젝트가 더 복잡해졌을 때는 각 맞는 State에서 상속받아서 두는 것이 더 좋음.
        input.PlayerActions.Attack.performed += OnAttackPerformed;
        input.PlayerActions.Attack.canceled += OnAttackCanceled;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.Player.Input;
        input.PlayerActions.Movement.canceled -= OnMovementCanceled;
        input.PlayerActions.Run.started -= OnRunStarted;
        input.PlayerActions.Jump.started -= OnJumpStarted;

        input.PlayerActions.Attack.performed -= OnAttackPerformed;
        input.PlayerActions.Attack.canceled -= OnAttackCanceled;
    }

    protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnRunStarted(InputAction.CallbackContext context)
    {

    }

    protected virtual void OnJumpStarted(InputAction.CallbackContext context)
    {
        // 언제 점프를 하는가? => 땅에 있을 떄.
    }

    protected virtual void OnAttackPerformed(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = true;
    }

    protected virtual void OnAttackCanceled(InputAction.CallbackContext context)
    {
        stateMachine.IsAttacking = false;
    }
    private void ReadMovementInput()
    {
        // 여기서 Input을 처리해야되는데 안들어옴.
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
            ((movementDirection * movementSpeed) + stateMachine.Player.ForceReceiver.Movement) * Time.deltaTime
            );
        // movementDirection * movementSpeed => 입력값의 방향과 정해진 속도를 곱한 속도
        // stateMachine.Player.ForceReceiver.Movement => ForceReceiver에서 impact와 Velocity를 합친 값.
    }

    protected void ForceMove() // 디렉션 값을 고려하지 않고 적용하는 힘 처리
    {
        stateMachine.Player.Controller.Move(stateMachine.Player.ForceReceiver.Movement * Time.deltaTime);
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

    // 이전에는 애니메이션 이벤트를 처리할 때, 
    // => 스크립트를 달아주고, 애니메이션 이벤트를 걸어줘서 UI에서 작업을 함.
    // 이번에는 애니메이션의 플레이타임을 저장시킴.

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0); // 현재 애니메이션 스테이트 인포를 가져옴.
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0); // 다음 애니메이션 스테이트 인포를 가져옴.

        // 지금 이 애니메이션은 트랜지션 라인을 타고 있고,
        // 다음 태그가 tag라면, 다음 것에 대한 normalizeTime을 전달해줌.
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime; // normalizeTime : 애니메이션의 전체 재생 시간을 0 ~ 1로 나타내 현재 처리하고 있는 시간을 나타낸 시간 단위
        }
        // 트랜지션이 아니고, 내가 태그가 attack이라면 현재 애니메이션의 normalizeTime을 전달해줌.
        else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag)) 
        {
            return currentInfo.normalizedTime;
        }
        else
        {
            return 0f;
        }
    }
}

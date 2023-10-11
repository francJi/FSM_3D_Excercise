using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
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
    // Ű �Է� ó��
    protected virtual void AddInputActionsCallbacks()
    {
        PlayerInput input = stateMachine.Player.Input;
        input.PlayerActions.Movement.canceled += OnMovementCanceled;
        input.PlayerActions.Run.started += OnRunStarted;
        input.PlayerActions.Jump.started += OnJumpStarted;
        // ������ �����̴� ������ �̵��̰� ��� ����Ǳ� ������ �� ���� ���Ƶξ�����, 
        // ���Ŀ� ������Ʈ�� �� ���������� ���� �� �´� State���� ��ӹ޾Ƽ� �δ� ���� �� ����.
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
        // ���� ������ �ϴ°�? => ���� ���� ��.
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
        // ���⼭ Input�� ó���ؾߵǴµ� �ȵ���.
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
            ((movementDirection * movementSpeed) + stateMachine.Player.ForceReceiver.Movement) * Time.deltaTime
            );
        // movementDirection * movementSpeed => �Է°��� ����� ������ �ӵ��� ���� �ӵ�
        // stateMachine.Player.ForceReceiver.Movement => ForceReceiver���� impact�� Velocity�� ��ģ ��.
    }

    protected void ForceMove() // �𷺼� ���� ������� �ʰ� �����ϴ� �� ó��
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

    // �������� �ִϸ��̼� �̺�Ʈ�� ó���� ��, 
    // => ��ũ��Ʈ�� �޾��ְ�, �ִϸ��̼� �̺�Ʈ�� �ɾ��༭ UI���� �۾��� ��.
    // �̹����� �ִϸ��̼��� �÷���Ÿ���� �����Ŵ.

    protected float GetNormalizedTime(Animator animator, string tag)
    {
        AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0); // ���� �ִϸ��̼� ������Ʈ ������ ������.
        AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0); // ���� �ִϸ��̼� ������Ʈ ������ ������.

        // ���� �� �ִϸ��̼��� Ʈ������ ������ Ÿ�� �ְ�,
        // ���� �±װ� tag���, ���� �Ϳ� ���� normalizeTime�� ��������.
        if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
        {
            return nextInfo.normalizedTime; // normalizeTime : �ִϸ��̼��� ��ü ��� �ð��� 0 ~ 1�� ��Ÿ�� ���� ó���ϰ� �ִ� �ð��� ��Ÿ�� �ð� ����
        }
        // Ʈ�������� �ƴϰ�, ���� �±װ� attack�̶�� ���� �ִϸ��̼��� normalizeTime�� ��������.
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

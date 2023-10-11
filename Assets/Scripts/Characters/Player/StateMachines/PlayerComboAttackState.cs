using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboAttackState : PlayerAttackState
{
    private bool alreadyAppliedForce; // ���� �̹� ���� �����ߴ°�
    private bool alreadyAppliedCombo; // ���� �޺��� �����ߴ°�

    AttackInfoData attackInfoData;

    public PlayerComboAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        alreadyAppliedCombo = false; // ������ �ִ� ���鿡 ���� �ʱ�ȭ ����
        alreadyAppliedForce = false;

        int comboIndex = stateMachine.ComboIndex; // stateMachine���� comboIndex�� �����ͼ�
        attackInfoData = stateMachine.Player.Data.AttackData.GetAttackInfo(comboIndex); // �ش� comboIndex�� ���� ������ �ҷ���
        stateMachine.Player.Animator.SetInteger("Combo", comboIndex); // �ִϸ����Ϳ� ����.
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        if (!alreadyAppliedCombo) // �޺������� �������� ���� ���·� attackState�� ��ȯ�Ǿ��ٸ�
        {
            stateMachine.ComboIndex = 0; // �޺� �ʱ�ȭ
        }
    }

    // �޺����ÿ� ���� ó��
    private void TryComboAttack()
    {
        if (alreadyAppliedCombo) // �޺������� �����ٸ�
        {
            return;
        }

        if (attackInfoData.ComboStateIndex == -1) // ������ ���ݿ� �ش��ϴ� �ε������
        {
            return;
        }

        if (!stateMachine.IsAttacking) // ������ �����ٸ� (IsAttacking�� false���)
        {
            return;
        }

        alreadyAppliedCombo = true; // ���� ��찡 �ƴ϶��, true
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedCombo) 
        {
            return;
        }

        alreadyAppliedForce = true;
        // �޴� �� reset
        stateMachine.Player.ForceReceiver.Reset();
        // �ٶ󺸴� ���⿡ �ٽ� addForce ����
        stateMachine.Player.ForceReceiver.AddForce(stateMachine.Player.transform.forward * attackInfoData.Force);
    }

    public override void Update()
    {
        base.Update();

        ForceMove();

        // NormalizeTime ȣ��
        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");

        if (normalizedTime < 1f) // �ִϸ��̼��� �������̶��,
        {
            if (normalizedTime >= attackInfoData.ForceTransitionTime) // �ִϸ��̼��� ������ ���� �����ؾ��ϴ� �ð����� Ŀ����
            {
                TryApplyForce(); // ���� ����
            }
            if (normalizedTime >= attackInfoData.ComboTransitionTime) // �޺��� üũ�ϴ� �ð����� Ŀ����
            { 
                TryComboAttack(); // �޺��� ����
            }
        }
        else // �ִϸ��̼��� �÷��̰� �Ϸ���� ���
        {
            if (alreadyAppliedCombo) // �޺� ������ ���
            {
                stateMachine.ComboIndex = attackInfoData.ComboStateIndex; // ComboStateIndex�� ���� �޺��ε����� ���� ����
                stateMachine.ChangeState(stateMachine.ComboAttackState); // �޺����ý�����Ʈ�� �ٽ� ��ȯ
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComboAttackState : PlayerAttackState
{
    private bool alreadyAppliedForce; // 내가 이미 힘을 적용했는가
    private bool alreadyAppliedCombo; // 내가 콤보를 적용했는가

    AttackInfoData attackInfoData;

    public PlayerComboAttackState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        alreadyAppliedCombo = false; // 가지고 있는 값들에 대한 초기화 진행
        alreadyAppliedForce = false;

        int comboIndex = stateMachine.ComboIndex; // stateMachine에서 comboIndex를 가져와서
        attackInfoData = stateMachine.Player.Data.AttackData.GetAttackInfo(comboIndex); // 해당 comboIndex에 대한 정보를 불러옴
        stateMachine.Player.Animator.SetInteger("Combo", comboIndex); // 애니메이터에 적용.
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.ComboAttackParameterHash);

        if (!alreadyAppliedCombo) // 콤보어택을 적용하지 않은 상태로 attackState가 전환되었다면
        {
            stateMachine.ComboIndex = 0; // 콤보 초기화
        }
    }

    // 콤보어택에 대한 처리
    private void TryComboAttack()
    {
        if (alreadyAppliedCombo) // 콤보어택이 끝났다면
        {
            return;
        }

        if (attackInfoData.ComboStateIndex == -1) // 마지막 공격에 해당하는 인덱스라면
        {
            return;
        }

        if (!stateMachine.IsAttacking) // 공격이 끝났다면 (IsAttacking이 false라면)
        {
            return;
        }

        alreadyAppliedCombo = true; // 위의 경우가 아니라면, true
    }

    private void TryApplyForce()
    {
        if (alreadyAppliedCombo) 
        {
            return;
        }

        alreadyAppliedForce = true;
        // 받는 힘 reset
        stateMachine.Player.ForceReceiver.Reset();
        // 바라보는 방향에 다시 addForce 적용
        stateMachine.Player.ForceReceiver.AddForce(stateMachine.Player.transform.forward * attackInfoData.Force);
    }

    public override void Update()
    {
        base.Update();

        ForceMove();

        // NormalizeTime 호출
        float normalizedTime = GetNormalizedTime(stateMachine.Player.Animator, "Attack");

        if (normalizedTime < 1f) // 애니메이션이 진행중이라면,
        {
            if (normalizedTime >= attackInfoData.ForceTransitionTime) // 애니메이션의 시점이 힘을 적용해야하는 시간보다 커졌다
            {
                TryApplyForce(); // 힘을 적용
            }
            if (normalizedTime >= attackInfoData.ComboTransitionTime) // 콤보를 체크하는 시간보다 커졌다
            { 
                TryComboAttack(); // 콤보를 적용
            }
        }
        else // 애니메이션의 플레이가 완료됬을 경우
        {
            if (alreadyAppliedCombo) // 콤보 적용이 됬다
            {
                stateMachine.ComboIndex = attackInfoData.ComboStateIndex; // ComboStateIndex는 다음 콤보인덱스를 갖고 있음
                stateMachine.ChangeState(stateMachine.ComboAttackState); // 콤보어택스테이트로 다시 전환
            }
            else
            {
                stateMachine.ChangeState(stateMachine.IdleState);
            }
        }
    }
}

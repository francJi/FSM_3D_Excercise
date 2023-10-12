using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Enemy Enemy { get; }

    public Transform Target { get; private set; }
    //상태 가져오기
    public EnemyIdleState IdlingState { get; }
    public EnemyChasingState ChasingState { get; }
    public EnemyAttackState AttackState { get; }
    // 이동관련 태그
    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

    public EnemyStateMachine(Enemy enemy)
    {
        Enemy = enemy;
        // 더 큰 구조를 짜게 되면, 플레이어와 적들의 레퍼런스를 따오 어딘가 두게 됨.
        Target = GameObject.FindGameObjectWithTag("Player").transform;

        IdlingState = new EnemyIdleState(this);
        ChasingState = new EnemyChasingState(this);
        AttackState = new EnemyAttackState(this);

        MovementSpeed = enemy.Data.GroundedData.BaseSpeed;
        RotationDamping = enemy.Data.GroundedData.BaseRotationDamping;
    }
}
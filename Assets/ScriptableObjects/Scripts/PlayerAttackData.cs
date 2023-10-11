using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackInfoData
{
    // 콤보어택까지 처리하기 위해 공격에 대한 처리를 해줄 클래스

    // 공격 이름
    [field: SerializeField] public string AttackName { get; private set; }
    // 콤보 인덱스
    [field: SerializeField] public int ComboStateIndex { get; private set; }
    // 콤보가 유지되려면 언제까지 때려야하는가에 대한 시간
    [field: SerializeField][field: Range(0f, 1f)] public float ComboTransitionTime { get; private set; }
    // 언제까지 공격을 누르고 있어야하는가
    [field: SerializeField][field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }
    // 적용할 힘
    [field: SerializeField][field: Range(-10f, 10f)] public float Force { get; private set; }

    [field: SerializeField] public int Damage { get; private set; }

}

[Serializable]
public class PlayerAttackData
{
    // 콤보에 대한 종류들을 리스트로
    [field: SerializeField] public List<AttackInfoData> AttackInfoDatas { get; private set; } 
    // AttackInfo에 대한 카운트
    public int GetAttackInfoCount() { return AttackInfoDatas.Count; }
    // 인덱스를 제공해주면, 현재 사용하고 있는 데이터에 대해 알 수 있게
    public AttackInfoData GetAttackInfo(int index) { return AttackInfoDatas[index]; }

}

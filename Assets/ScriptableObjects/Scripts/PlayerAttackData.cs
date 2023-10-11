using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackInfoData
{
    // �޺����ñ��� ó���ϱ� ���� ���ݿ� ���� ó���� ���� Ŭ����

    // ���� �̸�
    [field: SerializeField] public string AttackName { get; private set; }
    // �޺� �ε���
    [field: SerializeField] public int ComboStateIndex { get; private set; }
    // �޺��� �����Ƿ��� �������� �������ϴ°��� ���� �ð�
    [field: SerializeField][field: Range(0f, 1f)] public float ComboTransitionTime { get; private set; }
    // �������� ������ ������ �־���ϴ°�
    [field: SerializeField][field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }
    // ������ ��
    [field: SerializeField][field: Range(-10f, 10f)] public float Force { get; private set; }

    [field: SerializeField] public int Damage { get; private set; }

}

[Serializable]
public class PlayerAttackData
{
    // �޺��� ���� �������� ����Ʈ��
    [field: SerializeField] public List<AttackInfoData> AttackInfoDatas { get; private set; } 
    // AttackInfo�� ���� ī��Ʈ
    public int GetAttackInfoCount() { return AttackInfoDatas.Count; }
    // �ε����� �������ָ�, ���� ����ϰ� �ִ� �����Ϳ� ���� �� �� �ְ�
    public AttackInfoData GetAttackInfo(int index) { return AttackInfoDatas[index]; }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player�� rigidbody�� ���� �ʱ� ������, ���� ���õ� ó���� ��ũ��Ʈ���� ����
// Controller�� �߷°� �ʿ��� �ٸ� �� ����
// ���߿� �̵��ϴ� ó���� �� ��, ForceReceiver���� ����� ���� ������ ������ �ؾ���. ==> ForceReceiver ��ü�δ� �ƹ��� ������ ��ġ�� ����
public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float drag = 0.3f;     // ����

    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity; // Movement�� Ramda�� ���� ���� ������

    private void Update()
    {
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else // ���� �ƴ϶��, �ӵ��� ��� ������(�߷°��ӵ�)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        // ���װ��� ������ ������ ���� ���ҽ�Ŵ. (impact���� current -> impact�� 0�� �� ������)
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);  
    }

    public void Reset() // impact 0, �ӵ� 0
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }

    public void AddForce(Vector3 force) // impact�� �� �߰�
    {
        impact += force;
    }

    public void Jump(float jumpForce) // jumpForce�� �ӵ��� �߰�
    {
        verticalVelocity += jumpForce;
    }
}

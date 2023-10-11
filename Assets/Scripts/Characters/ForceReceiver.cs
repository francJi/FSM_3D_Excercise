using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player가 rigidbody를 쓰지 않기 때문에, 힘에 관련된 처리를 스크립트에서 진행
// Controller에 중력과 필요한 다른 힘 적용
// 나중에 이동하는 처리를 할 때, ForceReceiver에서 계산한 값을 가지고 적용을 해아함. ==> ForceReceiver 자체로는 아무런 영향을 끼치지 않음
public class ForceReceiver : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float drag = 0.3f;     // 저항

    private Vector3 dampingVelocity;
    private Vector3 impact;
    private float verticalVelocity;

    public Vector3 Movement => impact + Vector3.up * verticalVelocity; // Movement를 Ramda를 통해 값을 가져옴

    private void Update()
    {
        if (verticalVelocity < 0f && controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else // 땅이 아니라면, 속도를 계속 더해줌(중력가속도)
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
        // 저항값을 가지고서 찬찬히 값을 감소시킴. (impact값이 current -> impact가 0이 될 때까지)
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dampingVelocity, drag);  
    }

    public void Reset() // impact 0, 속도 0
    {
        impact = Vector3.zero;
        verticalVelocity = 0f;
    }

    public void AddForce(Vector3 force) // impact에 값 추가
    {
        impact += force;
    }

    public void Jump(float jumpForce) // jumpForce를 속도에 추가
    {
        verticalVelocity += jumpForce;
    }
}

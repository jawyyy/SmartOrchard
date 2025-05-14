using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotController : MonoBehaviour
{
    public Rigidbody rb;

    [Header("Wheel Transforms (for visual rotation)")]
    public Transform leftWheel;
    public Transform rightWheel;

    public float motorForce = 500f;
    public float turnTorque = 50f;
    public float wheelSpinSpeed = 1000f;

    [Header("Input Values")] // [-1, 1] 범위
    public float forwardInput;
    public float turnInput;

    void Start()
    {
        // 무게중심을 아래로 내리기 위해 Center of Mass 설정 (X,Z는 그대로, Y는 -0.5로 내림)
        rb.centerOfMass = new Vector3(0f, -0.5f, 0f);
    }

    void Update()
    {
        // 방향키 입력 처리
        forwardInput = Input.GetAxis("Vertical");   // W/S
        turnInput = Input.GetAxis("Horizontal");    // A/D

        // 시각적 회전 연출 (바퀴 Mesh만 회전)
        if (leftWheel)  leftWheel.Rotate(Vector3.right,  wheelSpinSpeed * forwardInput * Time.deltaTime);
        if (rightWheel) rightWheel.Rotate(Vector3.left,  wheelSpinSpeed * forwardInput * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // 직진/후진 힘 적용
        rb.AddForce(transform.forward * forwardInput * motorForce);

        // 회전 토크 적용 (Yaw 방향)
        rb.AddTorque(Vector3.up * turnInput * turnTorque);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneHamsterBallController : MonoBehaviour
{
    private float maxForce = 2.0f; // 최대 힘
    private float forceIncreaseRate = 4f; // 가속도
    private float currentForce = 0f;
    private float dragForce = 0.5f; // 마찰력
    private Rigidbody rb;
    private Vector3 forceDirection;
    private bool isAnyKeyPressed;
    private Camera mainCamera; // 메인 카메라

    private Vector3 resetPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = dragForce;
        mainCamera = Camera.main;
        resetPosition = transform.position;
        Debug.Log($"resetPosition: {resetPosition}");
    }

    void Update()
    {
        forceDirection = Vector3.zero;
        isAnyKeyPressed = false;

        // 카메라 기준으로 방향 설정
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        // 카메라의 상하 회전은 무시하기 위해 y값을 0으로 설정
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        if (Input.GetKey(KeyCode.UpArrow))
        {
            forceDirection -= right;
            isAnyKeyPressed = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            forceDirection += right;
            isAnyKeyPressed = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            forceDirection += forward;
            isAnyKeyPressed = true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            forceDirection -= forward;
            isAnyKeyPressed = true;
        }

        // 방향 정규화 (대각선 이동시 속도 일정하게)
        if (forceDirection != Vector3.zero)
        {
            forceDirection.Normalize();
        }

        // 키를 누르고 있으면 힘 증가
        if (isAnyKeyPressed)
        {
            currentForce += forceIncreaseRate * Time.deltaTime;
            currentForce = Mathf.Min(currentForce, maxForce);
            rb.AddForce(forceDirection * currentForce, ForceMode.Force);
        }
        else
        {
            // 키를 떼면 힘 초기화
            currentForce = 0f;
        }

        // 화면 밖으로 나갔는지 체크
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(transform.position);

        if (
            viewportPoint.x < 0.32f
            || viewportPoint.x > 0.53f
            || viewportPoint.z < 32.0f
            || viewportPoint.z > 40.0f
        )
        {
            // 포지션 초기화
            transform.position = resetPosition;
            // 속도 초기화
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            currentForce = 0f;
        }
    }
}

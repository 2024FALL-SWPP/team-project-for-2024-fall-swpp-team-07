using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSceneHamsterBallController : MonoBehaviour
{
    private float forceMagnitude;
    private float currentForce = 0f;        // 현재 적용되는 힘
    private float forceIncreaseRate = 20f;  // 힘이 증가하는 속도
    private float maxForce = 20f;         // 최대 힘 // 키 입력 1초 간 가속
    private Rigidbody rb;
    private Vector3 forceDirection;
    private bool isAnyKeyPressed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // 방향키 입력 확인
        forceDirection = Vector3.zero;
        isAnyKeyPressed = false;
        
        // 수평 방향 (좌우) 확인
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            forceDirection += new Vector3(1f, 0f, 0f);
            isAnyKeyPressed = true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            forceDirection += new Vector3(-1f, 0f, 0f);
            isAnyKeyPressed = true;
        }
        
        // 수직 방향 (상하) 확인
        if (Input.GetKey(KeyCode.UpArrow))
        {
            forceDirection += new Vector3(0f, 0f, -1f);
            isAnyKeyPressed = true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            forceDirection += new Vector3(0f, 0f, 1f);
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

        if (transform.position.y < -10f)
        {
            // 캐릭터 추락 시 비활성화
            GameObject.Find("Hamster3").SetActive(false);
        }
    }
}

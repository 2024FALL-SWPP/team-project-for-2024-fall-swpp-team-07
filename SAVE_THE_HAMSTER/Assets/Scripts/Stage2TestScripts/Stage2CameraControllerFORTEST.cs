using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2CameraControllerFORTEST : MonoBehaviour
{

    public GameObject ball; // 공 오브젝트
    private float offsetZ = 8.0f; // 공과 카메라 사이의 거리
    private float offsetY = 1.0f; // 공과 카메라 사이의 거리

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // 키보드 입력 R을 통해 카메라 회전
        {
            // 현재 회전에 Y축으로 90도 추가
            transform.Rotate(0f, 90f, 0f);
        }
        // 공의 위치로부터 일정 거리 뒤에 카메라 위치 설정
        transform.position = ball.transform.position - offsetZ * transform.forward + offsetY * transform.up;
    }

    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCamera;
    public Camera subCamera;
    public GameObject cannon; //대포 전체
    public Vector3 offset1;
    public Vector3 offset2;
    private bool isSpaceBar;

    private GameObject ball;

    void Start()
    {
        isSpaceBar = false;
        mainCamera.enabled = false;
        subCamera.enabled = true;
       
    }

    // Update is called once per frame
    void Update()
    {
        subCamera.transform.position = cannon.transform.position + new Vector3(-10f,5f,-20f);
         // 좌우,위아래 방향키 입력이 있을 때 서브 카메라로 전환
        if (!isSpaceBar && (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow)||Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
        {
            subCamera.enabled = true;
            mainCamera.enabled = false;
        }

        // 스페이스바 입력이 있을 때 메인 카메라로 전환
        if (Input.GetKey(KeyCode.Space))
        {
            isSpaceBar = true;
            subCamera.enabled = false;
            mainCamera.enabled = true;
            Vector3 desiredPosition = cannon.transform.position + offset1;

            mainCamera.transform.position = desiredPosition;
        
        }
        
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");

        // 조건에 맞는 공 찾기
        foreach (GameObject b in balls)
        {
            if (b.transform.position.y > cannon.transform.position.y)
            {
                ball = b; // 조건에 맞는 공 지정
                break; // 첫 번째 조건에 맞는 공을 찾으면 반복 종료
            }
        }
        if(ball != null){
            
            Vector3 targetPosition = ball.transform.position + offset2;
            mainCamera.transform.position = targetPosition;
        }
        
    }
    
        
    }
            



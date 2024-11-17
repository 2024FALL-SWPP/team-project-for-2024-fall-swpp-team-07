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

    void Start()
    {
        isSpaceBar = false;
        mainCamera.enabled = false;
        subCamera.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
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

        GameObject ball = GameObject.FindWithTag("Ball"); //발사되는 공, Ball tag 추가 필요
        if(ball != null){
        Vector3 targetPosition = ball.transform.position + offset2;
        mainCamera.transform.position = targetPosition;
        }
            
    }
}

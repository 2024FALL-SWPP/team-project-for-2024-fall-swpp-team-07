using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1PopUpManager : PopUpManager
{
    // Stage1 최초 애니메이션 소요 시간
    private float stage1AnimationTime = 10.0f;
    //여유시간
    private float stage1ExtraTime = 1.0f;
    // 소요시간 측정용
    private float stage1Timer = 0.0f;
    // 팝업 조건 변수
    bool lKeyPressed = false;
    bool leftArrowPressed = false;
    bool rightArrowPressed = false;
    bool upArrowPressed = false;
    bool downArrowPressed = false;

    // stage별 구현 부분
    protected override void PopUp()
    {
        Debug.Log(getTurns());
        bool isPopedUp = getIsPopedUp();
        // if (!disableTutorial) { }

        // 대포 생성 튜토리얼 팝업 조건 -> 애니메이션 카메라 움직임 완료 시(10초)
        if (!getPopedPopUp(0) && !isPopedUp)
        {
            stage1Timer += Time.deltaTime;
            if (stage1Timer >= stage1AnimationTime + stage1ExtraTime)
            {
                PopUpList[0].SetActive(true);
                setPopedPopUp(0, true);
                stage1Timer = 0.0f;
            }
        }

        // 대포 좌우이동 튜토리얼 팝업 조건 -> L키 누르고 3초 후
        // Stage1Scene 입장 후 최초 1회만 팝업
        if (!getPopedPopUp(1) && getPopedPopUp(0) && !isPopedUp)
        {
            if (!lKeyPressed && Input.GetKeyDown(KeyCode.L))
            {
                lKeyPressed = true;
            }
            if (lKeyPressed)
            {
                stage1Timer += Time.deltaTime;
                if (stage1Timer >= stage1ExtraTime)
                {
                    PopUpList[1].SetActive(true);
                    setPopedPopUp(1, true);
                    stage1Timer = 0.0f;
                }
            }
        }

        // 대포 상하이동 튜토리얼 팝업 조건 
        // -> 대포 좌우이동 튜토리얼 팝업 종료 후, 좌우키 누르고 3초 후
        // Stage1Scene 입장 후 최초 1회만 팝업
        if (!getPopedPopUp(2) && getPopedPopUp(1) && getPopedPopUp(0) && !isPopedUp)
        {
            // 좌우 화살표 키 눌렀는지 체크
            if (!leftArrowPressed && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                leftArrowPressed = true;
            }
            if (!rightArrowPressed && Input.GetKeyDown(KeyCode.RightArrow))
            {   
                rightArrowPressed = true;
            }

            // 좌우 화살표 키 눌렀으면 시간 측정 시작
            if (leftArrowPressed && rightArrowPressed)
            {
                stage1Timer += Time.deltaTime;
                if (stage1Timer >= stage1ExtraTime)
                {
                    PopUpList[2].SetActive(true);
                    setPopedPopUp(2, true);
                    stage1Timer = 0.0f;
                }
            }
        }

        // 대포 발사 튜토리얼 팝업 조건 
        // -> 대포 상하이동 튜토리얼 팝업 종료 후, 상하키 누르고 3초 후
        // Stage1Scene 입장 후 최초 1회만 팝업
        if (!getPopedPopUp(3) && getPopedPopUp(2) && getPopedPopUp(1) && getPopedPopUp(0) 
            && !isPopedUp)
        {
            // 상하 화살표 키 눌렀는지 체크
            if (!upArrowPressed && Input.GetKeyDown(KeyCode.UpArrow))
            {
                upArrowPressed = true;
            }
            if (!downArrowPressed && Input.GetKeyDown(KeyCode.DownArrow))
            {   
                downArrowPressed = true;
            }

            // 상하 화살표 키 눌렀으면 시간 측정 시작
            if (upArrowPressed && downArrowPressed)
            {
                stage1Timer += Time.deltaTime;
                if (stage1Timer >= stage1ExtraTime)
                {
                    PopUpList[3].SetActive(true);
                    setPopedPopUp(3, true);
                    stage1Timer = 0.0f;
                }
            }
        }

        // 햄스터 미세이동 holdhold 튜토리얼 팝업 조건
        // -> 햄스터 공 발사 후 정지한 직후
        // Stage1Scene 입장 후 최초 1회만 팝업
        // if (!getPopedPopUp(4) && getPopedPopUp(3) && getPopedPopUp(2) && getPopedPopUp(1) 
        //     && getPopedPopUp(0) && !isPopedUp)
        // {
        //     if (true) //햄스터 공 발사 후 정지
        //     {
        //         PopUpList[4].SetActive(true);
        //         setPopedPopUp(4, true);
        //     }
        // }
        setPopedPopUp(4, true);

        // 공 전환 튜토리얼 팝업 조건
        // -> 두번째 턴 시작 3초 후
        // Stage1Scene 입장 후 최초 1회만 팝업
        if (!getPopedPopUp(5) && getPopedPopUp(4) && getPopedPopUp(3) && getPopedPopUp(2) 
            && getPopedPopUp(1) && getPopedPopUp(0) && !isPopedUp)
        {
            if (getTurns() == 2) // (턴 종료 후 대포 생성되며 두번째 턴 시작)
            {
                stage1Timer += Time.deltaTime;
                if (stage1Timer >= stage1ExtraTime)
                {
                    PopUpList[5].SetActive(true);
                    setPopedPopUp(5, true);
                    stage1Timer = 0.0f;
                }
            }
        }
    }

}
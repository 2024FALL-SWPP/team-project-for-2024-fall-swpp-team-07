using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1PopUpManager : PopUpManager
{
    // Stage1 최초 애니메이션 소요 시간
    private float stage1AnimationTime = 10.0f;
    //여유시간
    private float stage1ExtraTime = 3.0f;
    // 소요시간 측정용
    private float stage1Timer = 0.0f;
    protected override void PopUp()
    {
        // 대포 좌우이동 튜토리얼 팝업 조건 -> 애니메이션 카메라 움직임 완료 시(10초)
        // Stage1Scene 입장 후 최초 1회만 팝업
        if (!getPopedPopUp(0))
        {
            stage1Timer += Time.deltaTime;
            if (stage1Timer >= stage1AnimationTime + stage1ExtraTime)
            {
                PopUpList[0].SetActive(true);
                setPopedPopUp(0, true);
                stage1Timer = 0.0f;
            }
        }

        // 대포 상하이동 튜토리얼 팝업 조건 
        // -> 좌우키 누르고 3초 후 + 대포 좌우이동 튜토리얼 팝업 종료 시
        // Stage1Scene 입장 후 최초 1회만 팝업
        if (!getPopedPopUp(1) && getPopedPopUp(0) && !PopUpList[0].activeSelf)
        {
            // 좌우 화살표 키 눌렀는지 체크
            bool leftArrowPressed = false;
            bool rightArrowPressed = false;
            if (!leftArrowPressed && Input.GetKeyDown(KeyCode.LeftArrow))
            {
                leftArrowPressed = true;
            }
            if (!rightArrowPressed && Input.GetKeyDown(KeyCode.RightArrow))
            {   
                rightArrowPressed = true;
            }
            //키보드 입력 인식 안되는 듯.. 11/23 16:17

            // 좌우 화살표 키 눌렀으면 시간 측정 시작
            if (leftArrowPressed && rightArrowPressed)
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

        
    }
}
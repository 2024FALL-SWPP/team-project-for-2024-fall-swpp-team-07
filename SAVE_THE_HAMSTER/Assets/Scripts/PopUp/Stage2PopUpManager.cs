using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2PopUpManager : PopUpManager
{
    private float popUpDistance = 30f; // 지형 오브젝트로 부터 일정 거리 이하로 다가갔을 때 팝업

    // Stage1 최초 애니메이션 소요 시간
    private float stage2AnimationTime = 10.0f;

    //여유시간
    private float stage2ExtraTime = 1.0f;

    // 소요시간 측정용
    private float stage2Timer = 0.0f;

    public GameObject[] terrain; // 0: Sand, 1: lava  2: water(물이 너무 커서 물 앞의 오브젝트 할당 예정)
    public GameObject cannon;

    // stage별 구현 부분
    protected override void PopUp()
    {
        bool isPopedUp = GetIsPopedUp();
        // if (!disableTutorial) { }

        // 모래 지형 튜토리얼 팝업 조건 -> 모래 지형 접근 시
        if (!GetPopedPopUp(0) && !isPopedUp)
        {
            stage2Timer += Time.deltaTime;
            if (stage2Timer >= stage2AnimationTime + stage2ExtraTime)
            {
                if (Distance(terrain[0]) <= popUpDistance)
                {
                    PopUpList[0].SetActive(true);
                    SetPopedPopUp(0, true);
                    stage2Timer = 0.0f;
                }
            }
        }

        // 용암 지형 튜토리얼 팝업 조건 -> 용암 지형 접근 시
        if (!GetPopedPopUp(1) && GetPopedPopUp(0) && !isPopedUp)
        {
            stage2Timer += Time.deltaTime;
            if (stage2Timer >= stage2AnimationTime + stage2ExtraTime)
            {
                if (Distance(terrain[1]) <= popUpDistance)
                {
                    PopUpList[1].SetActive(true);
                    SetPopedPopUp(1, true);
                    stage2Timer = 0.0f;
                }
            }
        }

        // 물 지형 튜토리얼 팝업 조건 -> 물 지형 접근 시
        if (!GetPopedPopUp(2) && GetPopedPopUp(1) && GetPopedPopUp(0) && !isPopedUp)
        {
            stage2Timer += Time.deltaTime;
            if (stage2Timer >= stage2AnimationTime + stage2ExtraTime)
            {
                if (Distance(terrain[2]) <= popUpDistance)
                {
                    PopUpList[2].SetActive(true);
                    SetPopedPopUp(2, true);
                    stage2Timer = 0.0f;
                }
            }
        }
    }

    private float Distance(GameObject terrain)
    {
        return Mathf.Abs(terrain.transform.position.z - cannon.transform.position.z); //z축 차이만 비교
    }
}

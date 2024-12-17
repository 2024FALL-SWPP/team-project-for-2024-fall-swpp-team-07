using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage3PopUpManager : PopUpManager
{
    private float popUpDistance = 30f; // 지형 오브젝트로 부터 일정 거리 이하로 다가갔을 때 팝업

    // Stage1 최초 애니메이션 소요 시간
    private float stage3AnimationTime = 10.0f;

    //여유시간
    private float stage3ExtraTime = 1.0f;

    // 소요시간 측정용
    private float stage3Timer = 0.0f;

    public GameObject[] environment; // 0: Fan, 1: BrickWall
    public GameObject cannon;

    // stage별 구현 부분
    protected override void PopUp()
    {
        bool isPopedUp = GetIsPopedUp();
        // if (!disableTutorial) { }

        // 선풀기 환경요소 튜토리얼 팝업 조건 -> 선풍기 환경요소 접근 시
        if (!GetPopedPopUp(0) && !isPopedUp)
        {
            stage3Timer += Time.deltaTime;
            if (stage3Timer >= stage3AnimationTime + stage3ExtraTime)
            {
                if (Distance(environment[0]) <= popUpDistance)
                {
                    PopUpList[0].SetActive(true);
                    SetPopedPopUp(0, true);
                    stage3Timer = 0.0f;
                }
            }
        }

        // 벽 환경요소 튜토리얼 팝업 조건 -> 벽 환경요소 접근 시
        if (!GetPopedPopUp(1) && GetPopedPopUp(0) && !isPopedUp)
        {
            stage3Timer += Time.deltaTime;
            if (stage3Timer >= stage3AnimationTime + stage3ExtraTime)
            {
                if (Distance(environment[1]) <= popUpDistance)
                {
                    PopUpList[1].SetActive(true);
                    SetPopedPopUp(1, true);
                    stage3Timer = 0.0f;
                }
            }
        }
    }

    private float Distance(GameObject environment)
    {
        float distanceZ = Mathf.Abs(environment.transform.position.z - cannon.transform.position.z); //z축 차이만 비교
        float distanceX = Mathf.Abs(environment.transform.position.x - cannon.transform.position.x); //x축 차이만 비교
        return Mathf.Sqrt(distanceZ * distanceZ + distanceX * distanceX); // 두 점 사이의 거리 계산
    }
}

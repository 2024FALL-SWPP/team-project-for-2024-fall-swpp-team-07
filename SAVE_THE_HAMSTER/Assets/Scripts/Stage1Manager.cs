using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : StageManager
{
    // This script takes care of all necessary info for stage 1.

    private int stageIndex = 0; //stage {1, 2, 3} <-> index {0, 1, 2}

    protected override void ReadyGame()
    {
        SetLifeLeft(stageIndex);
        ResetTurn();
        SetAlmondStatusDefault(GetTotalAlmond(stageIndex));
        // canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
        // 버튼 안 눌려서 캔버스 추가한 거
        // exitButton = GameObject.Find("ExitButton");
        // exitButton.SetActive(false);
        SetAlmondStatusDefault(GetTotalAlmond(stageIndex));
        SetTimerActive(false);
        SetCurrentTime(0);
        cannon.SetActive(false);
    }

    protected override void FinishGame()
    {
        // almondStatus, # of fires (totalLife - lifeLeft), _playTime 저장하기
        // 다음 씬으로 넘어가기
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : StageManager
{
    // This script takes care of all necessary info for stage 1.

    private int stageIndex = 0; //stage {1, 2, 3} <-> index {0, 1, 2}

    protected override void ReadyGame()
    {
        // stageIndex에 맞게 목숨 초기화
        SetLifeLeft(stageIndex);
        // 턴 초기화
        ResetTurn();
        SetAlmondStatusDefault(GetTotalAlmond(stageIndex));
        // UI 숨김
        canvas.SetActive(false);
        cannon.SetActive(false);
        // 타이머 초기화
        ResetTimer();
    }

    protected override void FinishGame()
    {
        // almondStatus, # of fires (totalLife - lifeLeft), _playTime 저장하기
        float finalTime = GetPlayTime();
        Debug.Log("Final Time: " + finalTime);
    }
}

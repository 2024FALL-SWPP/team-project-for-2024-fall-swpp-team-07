using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.example;
using com.example.Models;
using UnityEngine;

public class Stage1Manager : StageManager
{
    // This script takes care of all necessary info for stage 1.
    private int stageIndex = 0;
    public GameObject[] almonds = new GameObject[3]; // stage 1에서는 아몬드 3개
    public GameObject[] almondUI = new GameObject[3]; // stage 1에서는 아몬드 3개
    public GameObject[] almondUI_disabled = new GameObject[3]; // stage 1에서는 아몬드 3개
    private bool[] almondStatus = new bool[3]; // 아몬드 획득 여부
    private int obtainedAlmonds = 0; // 획득한 아몬드 수

    private void initializeAlmonds()
    {
        for (int i = 0; i < 3; i++)
        {
            if (!almondStatus[i])
            {
                almonds[i].SetActive(true);
                almondUI[i].SetActive(true);
                almondUI_disabled[i].SetActive(false);
            }
            else
            {
                almonds[i].SetActive(false);
                almondUI[i].SetActive(false);
                almondUI_disabled[i].SetActive(true);
            }
        }
    }

    private async Task<bool[]> GetAlmondStatus()
    {
        try
        {
            var client = SupabaseManager.Instance.Supabase();
            if (client != null && client.Auth.CurrentSession != null)
            {
                var userId = client.Auth.CurrentSession.User.Id;
                var actualStageIndex = stageIndex + 1;
                var stageStatus = await client
                    .From<UserStageStatus>()
                    .Select("almond_status")
                    .Where(x => x.user_id == userId && x.stage_id == actualStageIndex)
                    .Single();

                if (stageStatus != null)
                {
                    return stageStatus.almond_status;
                }
            }
            return new bool[3] { false, false, false };
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
            return new bool[3] { false, false, false };
        }
    }

    protected override async Task ReadyGame()
    {
        // stageIndex에 맞게 목숨 초기화
        SetLifeLeft(stageIndex);
        // 턴 초기화
        ResetTurn();
        // UI 숨김
        canvas.SetActive(false);
        cannon.SetActive(false);
        // 타이머 초기화
        ResetTimer();
        // 아몬드 상태 초기화 (일단 전부 false로)
        almondStatus = await GetAlmondStatus();

        initializeAlmonds();
    }

    protected override void FinishGame()
    {
        // almondStatus, # of fires (totalLife - lifeLeft), _playTime 저장하기
        float finalTime = GetPlayTime();
        Debug.Log("Final Time: " + finalTime);
    }

    public override void GetAlmond(int index)
    {
        almondStatus[index] = true;
        almondUI[index].SetActive(false);
        almondUI_disabled[index].SetActive(true);
        obtainedAlmonds++;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.example;
using com.example.Models;
using TMPro;
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

    // post stage canvas용 변수
    public TMP_Text[] ranking_names = new TMP_Text[8];
    public TMP_Text[] ranking_records = new TMP_Text[8];
    public TMP_Text clearText;
    public TMP_Text failText;
    public TMP_Text stageInfo;
    public TMP_Text stageRecord;
    public TMP_Text almondsNum;

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

    private async Task UpdateAlmondStatus()
    {
        try
        {
            var client = SupabaseManager.Instance.Supabase();
            if (client != null && client.Auth.CurrentSession != null)
            {
                var userId = client.Auth.CurrentSession.User.Id;
                var actualStageIndex = stageIndex + 1;

                // 기존 레코드 업데이트
                await client
                    .From<UserStageStatus>()
                    .Where(x => x.user_id == userId && x.stage_id == actualStageIndex)
                    .Set(x => x.almond_status, almondStatus)
                    .Update();

                if (obtainedAlmonds > 0)
                {
                    // 유저 아몬드 수 업데이트
                    var userProfile = await client
                        .From<UserProfile>()
                        .Select("almonds")
                        .Where(x => x.user_id == userId)
                        .Single();
                    int almonds = userProfile.almonds + obtainedAlmonds;

                    await client
                        .From<UserProfile>()
                        .Where(x => x.user_id == userId)
                        .Set(x => x.almonds, almonds)
                        .Update();
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    private async Task updateStageRecord(float finalTime, int numHits)
    {
        try
        {
            var client = SupabaseManager.Instance.Supabase();
            if (client != null && client.Auth.CurrentSession != null)
            {
                var userId = client.Auth.CurrentSession.User.Id;
                var actualStageIndex = stageIndex + 1;
                var userStageRecord = await client
                    .From<UserStageRecords>()
                    .Select(x => new object[] { x.clear_time, x.num_hits })
                    .Where(x => x.user_id == userId && x.stage_id == actualStageIndex)
                    .Single();

                // 일단 stage1_clear를 true로 업데이트
                await client
                    .From<UserProfile>()
                    .Where(x => x.user_id == userId)
                    .Set(x => x.stage1_clear, true)
                    .Update();

                if (userStageRecord != null)
                {
                    if (
                        userStageRecord.num_hits > numHits
                        || (
                            userStageRecord.num_hits == numHits
                            && userStageRecord.clear_time > finalTime
                        )
                    )
                    {
                        // 이미 존재하고, 기록이 더 좋다면 업데이트
                        var attempt_time = System.DateTime.Now.ToString("yy.MM.dd HH:mm:ss");
                        await client
                            .From<UserStageRecords>()
                            .Where(x => x.user_id == userId && x.stage_id == actualStageIndex)
                            .Set(x => x.clear_time, finalTime)
                            .Set(x => x.num_hits, numHits)
                            .Set(x => x.last_attempt, attempt_time)
                            .Update();
                    }
                }
                else
                {
                    var userProfile = await client
                        .From<UserProfile>()
                        .Select("nickname")
                        .Where(x => x.user_id == userId)
                        .Single();

                    // 처음 깨는 거라면 기록 등록
                    await client
                        .From<UserStageRecords>()
                        .Insert(
                            new UserStageRecords
                            {
                                user_id = userId,
                                stage_id = actualStageIndex,
                                clear_time = finalTime,
                                num_hits = numHits,
                                nickname = userProfile.nickname,
                                last_attempt = System.DateTime.Now.ToString("yy.MM.dd HH:mm:ss"),
                            }
                        );
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    private async Task updateRanking()
    {
        try
        {
            var client = SupabaseManager.Instance.Supabase();
            if (client != null && client.Auth.CurrentSession != null)
            {
                var userId = client.Auth.CurrentSession.User.Id;
                var actualStageIndex = stageIndex + 1;
                var userStageRecords = await client
                    .From<UserStageRecords>()
                    .Select(x => new object[] { x.user_id, x.nickname, x.num_hits, x.clear_time })
                    .Where(x => x.stage_id == actualStageIndex)
                    .Order(x => x.num_hits, Postgrest.Constants.Ordering.Ascending)
                    .Order(x => x.clear_time, Postgrest.Constants.Ordering.Ascending)
                    .Get();

                var numRecords = userStageRecords?.Models?.Count ?? 0;

                int i = 0;
                for (i = 0; i < numRecords; i++)
                {
                    var record = userStageRecords.Models[i];
                    ranking_names[i].text = record.nickname;
                    ranking_records[i].text = $"{record.num_hits}타/{record.clear_time:F3}초";

                    if (record.user_id == userId)
                    {
                        ranking_names[i].color = Color.yellow;
                        ranking_records[i].color = Color.yellow;
                    }
                }
                for (int j = i; j < 8; j++)
                {
                    ranking_names[j].text = "-";
                    ranking_records[j].text = "-";
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    protected override async Task ReadyGame()
    {
        SoundManager.Instance.PlayIngameBGM(1);
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

    protected override async Task FinishGame(bool clear)
    {
        float finalTime = GetPlayTime();

        // 새로 획득한 아몬드 수 업데이트 + DB에 반영
        almondsNum.text = $"    {obtainedAlmonds}";
        await UpdateAlmondStatus();

        stageInfo.text = $"Stage {stageIndex + 1} 도전 기록";
        if (!clear)
        {
            SoundManager.Instance.PlayFailFBX();
            // stage 실패 시 실패 UI 띄우기
            failText.gameObject.SetActive(true);
            clearText.gameObject.SetActive(false);
            stageRecord.text = "실패";
        }
        else
        {
            SoundManager.Instance.PlaySuccessFBX();
            // 성공 UI 띄우기 (이미 되어있음)
            stageRecord.text = $"{GetTurn()}타 / {finalTime:F3}초";
            // stage 클리어 시 기록 업데이트
            await updateStageRecord(finalTime, GetTurn());
        }

        // 랭킹 업데이트
        await updateRanking();

        ingameButtons.SetActive(false);
        postStageCanvas.SetActive(true);
        postStageBackgroundCanvas.SetActive(true);
    }

    public override void GetAlmond(int index)
    {
        almondStatus[index] = true;
        almondUI[index].SetActive(false);
        almondUI_disabled[index].SetActive(true);
        obtainedAlmonds++;
    }

    public override void UpdateTurnUI()
    {
        currentTurnText.text = $"{GetTurn() + 1} / {GetTotalLife(stageIndex)}";
    }
}

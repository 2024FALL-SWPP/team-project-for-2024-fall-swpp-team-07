using System;
using com.example;
using com.example.Models;
using TMPro;
using UnityEngine;

public class PreStageSceneManager : MonoBehaviour
{
    public TMP_Text[] ranking_names = new TMP_Text[8];
    public TMP_Text[] ranking_records = new TMP_Text[8];
    public TMP_Text stageInfo;
    public TMP_Text stageRecord;
    public TMP_Text almondsNum;
    private int stageNumber = 0;

    private async void Start()
    {
        // get stage number from SceneTransitionManager
        stageNumber = SceneTransitionManager.stageNumber;

        try
        {
            var client = SupabaseManager.Instance.Supabase();
            if (client != null && client.Auth.CurrentSession != null)
            {
                var userId = client.Auth.CurrentSession.User.Id;

                // 랭킹 업데이트
                var userStageRecords = await client
                    .From<UserStageRecords>()
                    .Select(x => new object[] { x.user_id, x.nickname, x.num_hits, x.clear_time })
                    .Where(x => x.stage_id == stageNumber)
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

                // 획득 가능한 아몬드 수 업데이트
                var userStageStatus = await client
                    .From<UserStageStatus>()
                    .Select("almond_status")
                    .Where(x => x.user_id == userId && x.stage_id == stageNumber)
                    .Single();

                if (userStageStatus != null)
                {
                    int almonds = 0;
                    foreach (var status in userStageStatus.almond_status)
                    {
                        if (!status)
                        {
                            almonds++;
                        }
                    }
                    almondsNum.text = $"{almonds}개";
                }
                else
                {
                    almondsNum.text = "3개";
                }

                // 스테이지 클리어 기록 업데이트
                stageInfo.text = $"Stage {stageNumber} 최고 도전 기록";
                var userStageRecord = await client
                    .From<UserStageRecords>()
                    .Select(x => new object[] { x.num_hits, x.clear_time, x.last_attempt })
                    .Where(x => x.user_id == userId && x.stage_id == stageNumber)
                    .Single();

                if (userStageRecord != null)
                {
                    stageRecord.text =
                        $"{userStageRecord.num_hits}타 / {userStageRecord.clear_time:F3}초 ({userStageRecord.last_attempt})";
                }
                else
                {
                    stageRecord.text = "기록 없음";
                }
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in PreStageScene: {e.Message}");
        }
    }
}

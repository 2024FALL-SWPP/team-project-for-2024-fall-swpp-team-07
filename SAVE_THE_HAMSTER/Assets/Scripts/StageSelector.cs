using System;
using System.Collections;
using System.Collections.Generic;
using com.example; // SceneTransitionManager 스크립트 사용을 위해 추가
using com.example.Models; // UserProfile 클래스 사용을 위해 추가
using TMPro; // UI 컴포넌트 사용을 위해 추가
using UnityEngine;

public class StageSelector : MonoBehaviour
{
    private int stageNumber = 1;
    private float cameraPositionX;
    public GameObject emphasizePlane;
    public TMP_Text stageText;

    private string[] stageExplanation =
    {
        "Stage1: 공을 발사하여 목표 지점에 도달하세요!",
        "Stage2: 다양한 지형과의 상호작용을 느껴보세요!",
        "Stage3: 장애물들을 극복해 목표 지점에 도달하세요!",
    };

    public GameObject Stage2Preview;
    public GameObject Stage3Preview;
    public GameObject Stage2Preview_Locked;
    public GameObject Stage3Preview_Locked;

    // Start is called before the first frame update
    private async void Start()
    {
        cameraPositionX = -3.0f;

        try
        {
            var client = SupabaseManager.Instance.Supabase();
            if (client != null && client.Auth.CurrentSession != null)
            {
                var userId = client.Auth.CurrentSession.User.Id;

                var userProfile = await client
                    .From<UserProfile>()
                    .Select(x => new object[] { x.stage1_clear, x.stage2_clear, x.stage3_clear })
                    .Where(x => x.user_id == userId)
                    .Single();

                if (userProfile != null)
                {
                    if (!userProfile.stage1_clear)
                    {
                        // stage2와 stage3를 잠금
                        Stage2Preview_Locked.SetActive(true);
                        Stage3Preview_Locked.SetActive(true);
                    }
                    else if (!userProfile.stage2_clear)
                    {
                        // stage3를 잠금
                        Stage2Preview.SetActive(true);
                        Stage3Preview_Locked.SetActive(true);
                    }
                    else
                    {
                        // 모든 스테이지를 해제
                        Stage2Preview.SetActive(true);
                        Stage3Preview.SetActive(true);
                    }
                }
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in StageSelectScene: {e.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (
            (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetAxis("Mouse ScrollWheel") > 0.0f)
            && stageNumber > 1
        )
        {
            stageNumber--;
            cameraPositionX -= 2.0f;
        }
        else if (
            (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetAxis("Mouse ScrollWheel") < 0.0f)
            && stageNumber < 3
        )
        {
            stageNumber++;
            cameraPositionX += 2.0f;
        }
        transform.position = new Vector3(
            cameraPositionX,
            transform.position.y,
            transform.position.z
        );
        emphasizePlane.transform.position = new Vector3(
            cameraPositionX,
            emphasizePlane.transform.position.y,
            emphasizePlane.transform.position.z
        );
        UpdateStageText();
    }

    void UpdateStageText()
    {
        stageText.text = stageExplanation[stageNumber - 1];
    }

    public int getStageNumber()
    {
        return stageNumber;
    }
}

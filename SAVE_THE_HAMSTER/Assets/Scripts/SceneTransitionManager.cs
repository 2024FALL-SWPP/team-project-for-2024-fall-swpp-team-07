using System.Collections;
using System.Collections.Generic;
using com.example;
using com.example;
using com.example.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static int stageNumber = 0;
    private StageSelector stageSelector;

    // Start is called before the first frame update
    void Start()
    {
        stageSelector = FindObjectOfType<StageSelector>();
        // if (stageSelector == null)
        // {
        //     Debug.LogError("StageSelector not found!");
        //     return;
        // }
    }

    // Update is called once per frame
    void Update() { }

    // public void GoToStartScene()
    // {
    //     //SceneManager.LoadScene("StartScene");
    // }

    public void GoToLoginScene()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void GoToLoadingScene()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    public void GoToStageSelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    public async void GoToPreStageScene()
    {
        if (stageSelector != null)
        {
            stageNumber = stageSelector.getStageNumber();
        }

        int allowedMaxStage = 1;
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
                    if (userProfile.stage1_clear)
                    {
                        allowedMaxStage = 2;
                    }
                    if (userProfile.stage2_clear)
                    {
                        allowedMaxStage = 3;
                    }
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error in StageSelectScene: {e.Message}");
        }

        if (stageNumber <= allowedMaxStage)
        {
            SceneManager.LoadScene("PreStageScene");
        }
        else
        {
            Debug.LogWarning("Stage is locked");
        }
    }

    public void GoToStageScene()
    {
        switch (stageNumber)
        {
            case 1:
                SceneManager.LoadScene("Stage1Scene");
                break;
            case 2:
                SceneManager.LoadScene("Stage2Scene");
                break;
            case 3:
                SceneManager.LoadScene("Stage3Scene");
                break;
            default:
                Debug.LogError("Invalid stage number: " + stageNumber);
                break;
        }
    }

    public void GoToGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void GoToStoreScene()
    {
        SceneManager.LoadScene("StoreScene");
    }

    public void GoToSettingScene()
    {
        SettingManager.Instance.OnOpen();
    }

    // Used for delivering stageNumber on PreStageScene and GameOverScene
    public int getStageNumber()
    {
        return stageNumber;
    }
}

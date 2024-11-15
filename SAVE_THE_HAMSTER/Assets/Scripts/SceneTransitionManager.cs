using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    private int stageNumber;
    private StageSelector stageSelector;
    // Start is called before the first frame update
    void Start() 
    {
        stageSelector = FindObjectOfType<StageSelector>();
    }

    // Update is called once per frame
    void Update() 
    {    }

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

    public void GoToPreStageScene()
    {
        stageNumber = stageSelector.getStageNumber();
        Debug.Log("GoToPreStageScene: " + stageNumber);
        // SceneManager.LoadScene("PreStageScene");
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
            case 4:
                SceneManager.LoadScene("Stage4Scene");
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
        //SceneManager.LoadScene("StoreScene");
    }

    public void GoToSettingScene()
    {
        SceneManager.LoadScene("StartScene");
        //SceneManager.LoadScene("SettingScene");
    }
}

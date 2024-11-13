using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTransitionManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // public void GoToStartScene()
    // {
    //     //SceneManager.LoadScene("StartScene");
    // }

    public void GoToLoginScene()
    {
        //SceneManager.LoadScene("LoginScene");
    }

    public void GoToLoadingScene()
    {
        SceneManager.LoadScene("LoadingScene");
    }

    public void GoToStageSelectScene()
    {
        SceneManager.LoadScene("StageSelectScene");
    }

    public void GoToStageScene()
    {
        //SceneManager.LoadScene("StageScene");
    }

    public void GoToStoreScene()
    {
        //SceneManager.LoadScene("StoreScene");
    }

    public void GoToSettingScene()
    {
        //SceneManager.LoadScene("SettingScene");
    }
}

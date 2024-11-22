using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // Setting Scene 에서 off시 true, Tutorial 팝업하지 않음
    // 공이 발사 중이라면 Tutorial 팝업하지 않음
    private static bool disableTutorial = false; 
    // 팝업할 Tutorial List
    public GameObject [] tutorialNote; 
    //한 번 팝업한 Tutorial은 true로 기록, 다시 팝업하지 않음, 씬 이동시 초기화
    private bool [] popedTutorial; 
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 다른 클래스에서 TutorialManager.disableTutorial 접근
    public void setTutorialOff()
    {
        disableTutorial = true;
    }

    public void setTutorialOn()
    {
        disableTutorial = false;
    }
}

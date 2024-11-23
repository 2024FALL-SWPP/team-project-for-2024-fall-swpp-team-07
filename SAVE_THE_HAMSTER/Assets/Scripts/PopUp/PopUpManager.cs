using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopUpManager : MonoBehaviour
{
    // Setting Scene 에서 off시 true, Tutorial 팝업하지 않음
    // 공이 발사 중이라면 Tutorial 팝업하지 않음
    private bool disableTutorial = false; 
    // 팝업할 Tutorial List
    public GameObject [] PopUpList; 
    //한 번 팝업한 Tutorial은 true로 기록, 다시 팝업하지 않음, 씬 이동시 초기화
    private bool [] popedPopUp; 
    
    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<Stage1Manager>로 추후 Stage종료 condition 만족 여부 접근 가능하게
        // popedPopUp 초기화
        popedPopUp = new bool[PopUpList.Length];
        for (int i = 0; i < PopUpList.Length; i++)
        {
            popedPopUp[i] = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        PopUp();
    }

    // public static bool GetKeyDown (KeyCode key)
    // {

    // }

    protected abstract void PopUp();

    // 다른 클래스에서 TutorialManager.disableTutorial 접근
    public void setTutorialOff()
    {
        disableTutorial = true;
    }

    public void setTutorialOn()
    {
        disableTutorial = false;
    }

    // 상속 클래스에서 참조
    public void setPopedPopUp(int index, bool value)
    {
        popedPopUp[index] = value;
    }

    public bool getPopedPopUp(int index)
    {
        return popedPopUp[index];
    }
}

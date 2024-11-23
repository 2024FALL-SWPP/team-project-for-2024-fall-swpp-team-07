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
    // 팝업 띄워져 있는가
    private bool isPopedUp = false;
    
    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<Setting>으로 disableTutorial 받아올 수 있도록
        // popedPopUp 초기화
        popedPopUp = new bool[PopUpList.Length];
        for (int i = 0; i < PopUpList.Length; i++)
        {
            popedPopUp[i] = disableTutorial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 팝업 띄워져 있는가
        checkPopUp();
        PopUp();
    }

    // public static bool GetKeyDown (KeyCode key)
    // {

    // }

    protected abstract void PopUp();

    private void checkPopUp()
    {
        isPopedUp = false;
        for (int i = 0; i < PopUpList.Length; i++)
        {
            if (PopUpList[i].activeSelf)
            {
                isPopedUp = true;
            }
        }
    }

    // 다른 클래스에서 TutorialManager.disableTutorial 접근
    // ***받아서 쓰기
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

    public bool getIsPopedUp()
    {
        return isPopedUp;
    }
}

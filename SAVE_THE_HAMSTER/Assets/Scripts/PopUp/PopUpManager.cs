using System.Collections;
using System.Collections.Generic;
using com.example;
using UnityEngine;

public abstract class PopUpManager : MonoBehaviour
{
    private SettingManager settingManager;
    private StageManager stageManager;

    // Setting Scene 에서 off시 true, Tutorial 팝업하지 않음
    // 공이 발사 중이라면 Tutorial 팝업하지 않음
    private bool disableTutorial = false;

    // 팝업할 Tutorial List
    public GameObject[] PopUpList;

    //한 번 팝업한 Tutorial은 true로 기록, 다시 팝업하지 않음, 씬 이동시 초기화
    private bool[] popedPopUp;

    // 팝업 띄워져 있는가
    private bool isPopedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        // GetComponent<Setting>으로 disableTutorial 받아올 수 있도록
        settingManager = SettingManager.Instance;
        // GetComponent<StageManager>()로 stageManager.turns 받아올 수 있도록
        stageManager = FindObjectOfType<StageManager>();
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
        // 튜토리얼 팝업 여부 가져오기
        if (settingManager != null)
        {
            disableTutorial = settingManager.disableTutorial;
        }
        // 팝업 띄워져 있는가
        CheckPopUp();
        if (!disableTutorial)
        {
            PopUp();
        }

        // Enter 키로 현재 활성화된 팝업 닫기
        if (Input.GetKeyDown(KeyCode.Return) && isPopedUp)
        {
            for (int i = 0; i < PopUpList.Length; i++)
            {
                if (PopUpList[i].activeSelf)
                {
                    PopUpList[i].SetActive(false);
                    break; // 첫 번째로 찾은 활성화된 팝업만 닫기
                }
            }
        }

        // 팝업 여부에 따라 타이머 일시정지
        if (isPopedUp)
        {
            stageManager.PauseTimer();
        }
        else
        {
            stageManager.ResumeTimer();
        }
    }

    protected abstract void PopUp();

    private void CheckPopUp()
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
    public void SetTutorialOff()
    {
        disableTutorial = true;
    }

    public void SetTutorialOn()
    {
        disableTutorial = false;
    }

    // 상속 클래스에서 참조
    public void SetPopedPopUp(int index, bool value)
    {
        popedPopUp[index] = value;
    }

    public bool GetPopedPopUp(int index)
    {
        return popedPopUp[index];
    }

    public bool GetIsPopedUp()
    {
        return isPopedUp;
    }

    public int GetTurn()
    {
        return stageManager.GetTurn();
    }
}

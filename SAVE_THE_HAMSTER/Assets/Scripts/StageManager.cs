using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageManager : MonoBehaviour
{
    // This script takes care of all necessary info for stages.

    private const int numberOfStages = 3;

    private static int [] totalLife = new int[numberOfStages] { 5, 5, 5 }; //get
    private int lifeLeft; //set 발사 시 life--;
    private int turns; //get 대포가 생성되는게 새로운 턴
    private int previousTurns = 0; // temp variable for 이전 턴 수
    private static int [] totalAlmond = new int[numberOfStages] { 5, 5, 5 }; //get
    private bool [] almondStatus; //set
    private bool isStart = false; //대포가 생성됐는지 //set

    private bool _timerActive = false; //set
    private float _currentTime = 0; //set

    private bool success = false; //set
    private bool failure = false; //set

    public GameObject[] balls;
    public GameObject cannon;
    public GameObject animationCamera;
    public GameObject cameraController;
    public GameObject canvas;

    CannonControl cannonControl;
    // 버튼 안 눌려서 캔버스 추가한 거
    // GameObject exitButton;

    public GameObject GetActiveBall()
    {
        foreach (GameObject ball in balls)
        {
            if (ball.activeSelf)
            {
                return ball;
            }
        }
        return null;
    }

    public void AnimationEnd(int animation)
    {
        // Starting Animation
        if (animation == 1)
        {
            StartGame();
        }

        if (animation == 2)
        {
            EndGame();
        }
    }

    // set values, status properly for each stage
    // call on StageManager.Start() during animation
    protected abstract void ReadyGame();

    // activate canvas, gamecamera, timer and etc for game
    // call on StageManager.AnimationEnd(1) 
    // by Assets/Addons/CameraPathCreator/Scripts/CPC_CameraPath.cs
    // right after animation
    // protected abstract void StartGame(); // 생각해보니 각 stage 별로 다를게 없을 듯
    void StartGame()
    {
        animationCamera.SetActive(false);
        canvas.SetActive(true);
        // 버튼 안 눌려서 캔버스 추가한 거
        // exitButton.SetActive(true);
        _timerActive = true;
        CameraControl cameraScript = cameraController.GetComponent<CameraControl>();
        cameraScript.enabled = true;
    }

    protected abstract void EndGame();
    // void EndGame()
    // {
    //     // somehow return/give [almondStatus, # of fires (totalLife - lifeLeft), _currentTime].
    // }

    public int getTotalLife(int stageIndex)
    {
        return totalLife[stageIndex];
    }

    public void setLifeLeft(int life)
    {
        lifeLeft = life;
    }  

    public void minusLifeLeft() // when turn over or respawn
    {
        lifeLeft--;
    }

    public int getLifeLeft() // for debuging
    {
        return lifeLeft;
    }

    public int getTurns()
    {
        return cannonControl.getTurns();
    } 

    public int getTotalAlmond(int stageIndex)
    {
        return totalAlmond[stageIndex];
    }

    public void setAlmondStatusDefault(int totalAlmond)
    {
        almondStatus = new bool[totalAlmond];
        for (int i = 0; i < totalAlmond; i++)
        {
            almondStatus[i] = false;
        }
    }

    public void setAlmondStatus(int almondNumber, bool value)
    {
        almondStatus[almondNumber] = value;
    }

    public void setIsStart(bool isStart)
    {
        isStart = isStart;
    }

    public void setTimerActive(bool timerActive)
    {
        _timerActive = timerActive;
    }

    public void setCurrentTime(float currentTime)
    {
        _currentTime = currentTime;
    }

    public void setSuccess(bool value)
    {
        success = value;
    }

    public void setFailure(bool value)
    {
        failure = value;
    }

    // Start is called before the first frame update
    void Start()
    {
        cannonControl = cannon.GetComponent<CannonControl>();
        // totalLife = new int [] {5, 5, 5};
        // totalAlmond = new int [] {1, 3, 5};

        ReadyGame();
        // lifeLeft = totalLife;
        // canvas = GameObject.Find("Canvas");
        // canvas.SetActive(false);
        // // 버튼 안 눌려서 캔버스 추가한 거
        // // exitButton = GameObject.Find("ExitButton");
        // // exitButton.SetActive(false);
        // almondStatus = new bool[totalAlmond]; // initializes to false. // TODO: change on enter by getting an array of almondStatus.
        // _timerActive = false;
        // _currentTime = 0;
        // cannon.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // 마지막 라이프 소멸 후 다음 턴이 되었을 때 실패임을 구현
        // 마지막 라이프가 소멸되며 골인하면 성공 처리, 실패 구현 안되게끔
        turns = getTurns();
        if (lifeLeft <= 0) 
        {
            if (turns == previousTurns + 1)
            {
                failure = true;
            }
        }
        previousTurns = turns;

        if (_timerActive) // 팝업 뜰 때 타이머 일시 정지 가능
        {
            _currentTime += Time.deltaTime;
            if (!isStart && Input.GetKeyDown(KeyCode.L))
            { //Launch의 의미 :L
                isStart = true;
                //AnimationEnd(1);
                cannon.SetActive(true); //발사 가능하게
            }
        }

        if (success && !failure) // 성공, 실패 중 먼저 성립하는 조건만 실행
        {
            Success();  
        }

        if (failure && !success) // 성공, 실패 중 먼저 성립하는 조건만 실행
        {
            Failure();
        }
    }

    // Success, Failure 함수는 스테이지 공통 애니메이션 구현
    // StartGame, EndGame 함수는 각 스테이지에 알맞게 variable 세팅, variable 전달
    // StartGame, EndGame 함수는 계정 별 스킨 세팅 및 계정 별 게임종료 후 정보 저장도 구현

    public void Success()
    {
        Debug.Log("Success");
    }

    public void Failure()
    {
        Debug.Log("Failure");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StageManager : MonoBehaviour
{
    // This script takes care of all necessary info for stages.
    // Flow: ReadyGame -> StartGame -> EndGame -> FinishGame
    // ReadyGame: 게임 준비 단계: stage 별로 초기 세팅
    // StartGame: 게임 시작 단계: 시작 카메라 애니메이션 종료 후, 캐논 활성화, 타이머 시작, 카메라 조절
    // EndGame: 게임 종료 단계: 타이머 정지, 캔버스 비활성화, 성공 시 햄스터 애니메이션 동시에 종료 카메라 애니메이션
    // FinishGame: 게임 기록 단계: 종료 카메라 애니메이션 종료 후, 성공 혹은 실패 처리 후 게임 기록 저장

    private const int numberOfStages = 3;

    private static int [] totalLife = new int[numberOfStages] { 5, 5, 5 }; //get
    private int lifeLeft; //set 발사 시 life--;
    private int turns; //get 대포가 생성되는게 새로운 턴
    private int previousTurns = 0; // temp variable for 이전 턴 수
    private static int [] totalAlmond = new int[numberOfStages] { 5, 5, 5 }; //get
    private bool [] almondStatus; //set
    private bool isStart = false; //대포가 생성됐는지 //set
    private bool end = false; // 게임 종료 여부, 종료 함수 한 번만 호출하기 위해

    private bool _timerActive = false; //set
    private float _currentTime = 0; //set, 타이머 시간
    private float _playTime; //get, 게임 플레이 시간

    private bool success = false; //set
    private bool failure = false; //set

    private HamsterAnimationControl hamsterAnimationControl;

    public GameObject[] balls;
    public GameObject chest;
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
            FinishGame();
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

    void EndGame(bool value) // value == success from CollisionDetection
    {
        CameraControl cameraScript = cameraController.GetComponent<CameraControl>();
        cameraScript.enabled = false;
        cannon.SetActive(false);
        GameObject lineRenderer = GameObject.Find("LineRenderer");
        lineRenderer.SetActive(false);
        _timerActive = false;
        _playTime = _currentTime;
        canvas.SetActive(false);
        if (value)
        {
            Success();
        }
        else
        {
            Failure();
        }
        // animationCamera.SetActive(true);
    }

    protected abstract void FinishGame();
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

    public float getPlayTime()
    {
        return _playTime;
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

        if ((success || failure) && !end)
        {
            if (almondStatus[0]) Debug.Log("almond");
            EndGame(success);
            end = true;
        }
    }

    // Success, Failure 함수는 스테이지 공통 애니메이션 구현
    // StartGame, EndGame 함수는 각 스테이지에 알맞게 variable 세팅, variable 전달
    // StartGame, EndGame 함수는 계정 별 스킨 세팅 및 계정 별 게임종료 후 정보 저장도 구현

    public void Success()
    {
        // 캐논 비활성화
        // 햄스터 볼 활성화, 체스트 앞으로 이동
        Rigidbody ballrb = balls[0].GetComponent<Rigidbody>();
        if (!balls[0].activeSelf)
        {
            for (int i = 1; i < balls.Length; i++)
            {
                balls[i].SetActive(false);
            }
            balls[0].SetActive(true);
            ballrb.useGravity = true;
        }
        ballrb.velocity = Vector3.zero; //체스트 앞에서 멈춘 후 위로 뛰며 회전하도록
        ballrb.angularVelocity = Vector3.zero;
        ballrb.Sleep();
        balls[0].transform.position = chest.transform.position + chest.transform.forward * 3f;
        hamsterAnimationControl = FindObjectOfType<HamsterAnimationControl>().GetComponent<HamsterAnimationControl>();
        hamsterAnimationControl.setSuccess(true);
        // 체스트 애니메이션
    }

    public void Failure()
    {
        Debug.Log("Failure");
    }
}

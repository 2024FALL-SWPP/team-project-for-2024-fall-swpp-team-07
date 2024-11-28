using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : StageManager
{
    // This script takes care of all necessary info for stage 1.

    // public int totalLife;
    // public int lifeLeft;
    // int totalAlmond = 1;
    // bool[] almondStatus;
    // private bool isStart = false; //대포가 생성됐는지

    private int stageIndex = 0; //stage {1, 2, 3} <-> index {0, 1, 2}
    // private int totalLife;
    // private int totalAlmond;


    // bool _timerActive = false;
    // float _currentTime = 0;

    // public GameObject[] balls;
    // public GameObject cannon;
    // public GameObject animationCamera;
    // public GameObject cameraController;
    // GameObject canvas;
    // 버튼 안 눌려서 캔버스 추가한 거
    // GameObject exitButton;

    // public GameObject GetActiveBall()
    // {
    //     foreach (GameObject ball in balls)
    //     {
    //         if (ball.activeSelf)
    //         {
    //             return ball;
    //         }
    //     }
    //     return null;
    // }

    // public void AnimationEnd(int animation)
    // {
    //     // Starting Animation
    //     if (animation == 1)
    //     {
    //         StartGame();
    //     }

    //     if (animation == 2)
    //     {
    //         EndGame();
    //     }
    // }

    protected override void ReadyGame()
    {
        setLifeLeft(getTotalLife(stageIndex));
        setAlmondStatusDefault(getTotalAlmond(stageIndex));
        // canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
        // 버튼 안 눌려서 캔버스 추가한 거
        // exitButton = GameObject.Find("ExitButton");
        // exitButton.SetActive(false);
        setAlmondStatusDefault(getTotalAlmond(stageIndex));
        setTimerActive(false);
        setCurrentTime(0);
        cannon.SetActive(false);
    }

    // void StartGame()
    // {
    //     animationCamera.SetActive(false);
    //     canvas.SetActive(true);
    //     // 버튼 안 눌려서 캔버스 추가한 거
    //     // exitButton.SetActive(true);
    //     _timerActive = true;
    //     CameraControl cameraScript = cameraController.GetComponent<CameraControl>();
    //     cameraScript.enabled = true;
    // }

    protected override void EndGame()
    {
        // somehow return/give [almondStatus, # of fires (totalLife - lifeLeft), _currentTime].
    }

    // public void GetAlmond(int almondNumber)
    // {
    //     almondStatus[almondNumber] = true;
    // }

    // Start is called before the first frame update
    // void Start()
    // {
    //     lifeLeft = totalLife;
    //     // canvas = GameObject.Find("Canvas");
    //     canvas.SetActive(false);
    //     // 버튼 안 눌려서 캔버스 추가한 거
    //     // exitButton = GameObject.Find("ExitButton");
    //     // exitButton.SetActive(false);
    //     almondStatus = new bool[totalAlmond]; // initializes to false. // TODO: change on enter by getting an array of almondStatus.
    //     setTimerActive(false);
    //     setCurrentTime(0);
    //     cannon.SetActive(false);
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     if (_timerActive)
    //     {
    //         _currentTime += Time.deltaTime;
    //         if (!getIsStart() && Input.GetKeyDown(KeyCode.L))
    //         { //Launch의 의미 :L
    //             setIsStart(true);
    //             AnimationEnd(1);
    //             cannon.SetActive(true); //발사 가능하게
    //         }
    //     }
    // }
}

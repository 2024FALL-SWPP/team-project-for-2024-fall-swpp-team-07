using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
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

    private static int[] totalLife = new int[numberOfStages] { 5, 5, 5 }; //get
    private int lifeLeft = 0; //set 발사 시 life--;
    private int currentTurn = 1;
    private int previousTurn = 0;
    private bool isStart = false; //대포가 생성됐는지 //set
    private bool end = false; // 게임 종료 여부, 종료 함수 한 번만 호출하기 위해

    // 타이머 및 시간 측정
    private bool _timerActive = false;
    private bool isAnimationFinished = false;
    private float _currentTime = 0;
    private float _playTime;
    public TMP_Text timerText;

    private bool success = false; //set
    private bool failure = false; //set

    // 종료 시 햄스터 위치로 메인 카메라를 이동시키는 변수들
    private bool isSuccessAnimation = false;
    private Transform targetBall;
    private bool isCameraPositioned = false;

    // poststage canvas
    public GameObject postStageCanvas;
    public GameObject postStageBackgroundCanvas;

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

    // set values, status properly for each stage
    // call on StageManager.Start() during animation
    protected abstract Task ReadyGame();

    // activate canvas, gamecamera, timer and etc for game
    // call by Assets/Addons/CameraPathCreator/Scripts/CPC_CameraPath.cs
    // right after animation
    public void StartGame()
    {
        animationCamera.SetActive(false);
        canvas.SetActive(true);
        _timerActive = true;
        isAnimationFinished = true;
        CameraControl cameraScript = cameraController.GetComponent<CameraControl>();
        cameraScript.enabled = true;
    }

    void EndGame(bool value)
    {
        CameraControl cameraScript = cameraController.GetComponent<CameraControl>();
        cameraScript.enabled = false;
        cannon.SetActive(false);
        GameObject lineRenderer = GameObject.Find("LineRenderer");
        lineRenderer.SetActive(false);
        canvas.SetActive(false);
        _timerActive = false;
        _playTime = _currentTime;

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

    protected abstract Task FinishGame(bool clear);

    public void SetLifeLeft(int stageIndex)
    {
        lifeLeft = totalLife[stageIndex];
    }

    public void DecreaseLifeLeft() // when turn over or respawn
    {
        lifeLeft--;
    }

    public int GetLifeLeft() // for debuging
    {
        return lifeLeft;
    }

    public int GetTurn()
    {
        return currentTurn;
    }

    public void IncreaseTurn()
    {
        currentTurn++;
    }

    public void ResetTurn()
    {
        currentTurn = 1;
        previousTurn = 0;
    }

    public int GetPreviousTurn()
    {
        return previousTurn;
    }

    public void UpdatePreviousTurn()
    {
        previousTurn = currentTurn;
    }

    public void SetIsStart(bool isStart)
    {
        isStart = isStart;
    }

    public void ResetTimer()
    {
        isAnimationFinished = false;
        _timerActive = false;
        _currentTime = 0;
        UpdateTimerUI();
    }

    public void PauseTimer()
    {
        _timerActive = false;
    }

    public void ResumeTimer()
    {
        if (isAnimationFinished && !end)
            _timerActive = true;
    }

    public float GetPlayTime()
    {
        return _playTime;
    }

    public void SetSuccess()
    {
        // 성공 플래그 세팅 및 타이머 정지
        _timerActive = false;
        success = true;
    }

    public void SetFailure()
    {
        // 실패 플래그 세팅 및 타이머 정지
        _timerActive = false;
        failure = true;
    }

    public abstract void GetAlmond(int index);

    private void UpdateTimerUI()
    {
        // 초 단위를 분:초 변환
        int minutes = Mathf.FloorToInt(_currentTime / 60f);
        int seconds = Mathf.FloorToInt(_currentTime % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // Start is called before the first frame update
    private async void Start()
    {
        cannonControl = cannon.GetComponent<CannonControl>();
        await ReadyGame();
    }

    // Update is called once per frame
    void Update()
    {
        // 타이머 업데이트
        if (_timerActive)
        {
            _currentTime += Time.deltaTime;
            UpdateTimerUI();

            if (!isStart && Input.GetKeyDown(KeyCode.Space))
            {
                isStart = true;
                cannon.SetActive(true); //발사 가능하게
            }
        }

        // 마지막 라이프 소멸 후 다음 턴이 되었을 때 실패임을 확인
        // 마지막 라이프가 소멸되며 골인하면 성공 처리, 실패 구현 안되게끔
        if (lifeLeft <= 0 && GetTurn() > GetPreviousTurn())
        {
            failure = true;
        }

        previousTurn = currentTurn;

        if ((success || failure) && !end)
        {
            EndGame(success);
            end = true;
        }
    }

    void LateUpdate()
    {
        if (isSuccessAnimation && targetBall != null)
        {
            Camera mainCamera = Camera.main;
            Vector3 targetPosition = targetBall.position + new Vector3(0f, 3f, -5f);

            if (!isCameraPositioned)
            {
                // 카메라를 목표 위치로 이동
                mainCamera.transform.position = Vector3.Lerp(
                    mainCamera.transform.position,
                    targetPosition,
                    Time.deltaTime * 2f
                );

                // 카메라가 목표 위치에 충분히 가까워졌는지 확인
                if (Vector3.Distance(mainCamera.transform.position, targetPosition) < 0.1f)
                {
                    isCameraPositioned = true;
                    mainCamera.transform.position = targetPosition; // 정확한 위치로 설정
                }
            }
            // 햄스터 볼을 바라보도록
            mainCamera.transform.LookAt(targetBall.position);
        }
    }

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
        hamsterAnimationControl = FindObjectOfType<HamsterAnimationControl>()
            .GetComponent<HamsterAnimationControl>();
        hamsterAnimationControl.SetSuccess();
        // 체스트 애니메이션, 파티클 효과도 추가하면 좋을듯

        // 카메라 이동
        isSuccessAnimation = true;
        targetBall = balls[0].transform;

        // Invoke 대신 코루틴 사용
        StartCoroutine(FinishGameAfterDelay(true));
    }

    private IEnumerator FinishGameAfterDelay(bool clear)
    {
        yield return new WaitForSeconds(10f);
        // 비동기 메서드 실행
        _ = FinishGame(clear);
    }

    public void Failure()
    {
        // 실패는 딜레이 없이 바로 실행
        _ = FinishGame(false);
    }
}

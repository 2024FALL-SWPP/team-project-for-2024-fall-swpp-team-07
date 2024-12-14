using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CannonControl : MonoBehaviour
{
    StageManager gm;
    public GameObject cannon; // 대포 전체
    private GameObject activeBall; //발사할 공
    public Transform firePoint; // 탄환 발사 지점
    public LineRenderer lineRenderer;
    public ParticleSystem explosion;
    public GameObject bigExplosion;
    public GameObject armatureParent; //Hamster armature의 parent객체
    public GameObject hamsterBall; //햄스터 공 전체
    private BallControl hamsterScript; //hamster armature의 부모 객체인 gameObject에 적용되어 있는 script
    private HamsterCollision hamsterCollisionScript;
    private CollisionDetection collisionScript;
    private StickyBallCollision stickyBallCollisionScript;

    //
    private const int N_TRAJECTORY_POINTS = 40;
    public float minForce = 0f; //최소 발사력
    public float maxForce = 5f; //최대 발사력

    //private float forceMultipler = 5f;// 발사력에 적용할 비례 상수
    private float forceIncreaseRate = 5f; //발사력 증가 속도

    //cf) (최대 발사력 - 최소 발사력)/발사력 증가 속도 = 1 / fillSpeed (GaugeControl.cs)
    private float maxUp = 60f;
    private float maxDown = 10f;

    //private float maxLeft = -120f;
    //private float maxRight = 120f;

    private float scaleMultiplier = 2.5f; // 공 대비 대포 전체 비율-> 추후에 아이템으로 공이 커지는 효과를 구현한다면 대포도 커지게 설정

    private Vector3 startPosition;
    private float initialXRotation;
    private float currentXRotation;

    private Quaternion initialLocalRotation; //canon의 localRotation값 저장

    public float force;

    private bool isForceIncreasing = true; //force가 증가하고 있는지 여부

    public GameObject canon; //포신

    public float rotationSpeed = 100f; // 회전 속도

    public int spaceBarCount; //spaceBar가 눌린 횟수

    private bool isRunning;
    private bool isGround; //공이 지면과 충돌했는지

    public bool spacePressed = false;

    private bool isRespawn;

    private bool isGameOver = false;

    private Vector3 prevBallPosition; //발사 직전 공 위치

    private Quaternion savedRotation;

    private Vector3 normal; //지형의 법선벡터

    void Start()
    {
        gm = FindObjectOfType<StageManager>();
        hamsterScript = armatureParent.GetComponent<BallControl>();
        hamsterCollisionScript = hamsterBall.GetComponent<HamsterCollision>();
        startPosition = transform.position; //대포의 시작 위치 저장
        initialLocalRotation = canon.transform.localRotation;
        initialXRotation = canon.transform.localRotation.eulerAngles.x;
        currentXRotation = initialXRotation;
        savedRotation = cannon.transform.rotation;
        lineRenderer.positionCount = N_TRAJECTORY_POINTS;
        // 선의 두께 설정
        lineRenderer.startWidth = 0.8f; // 시작 두께
        lineRenderer.endWidth = 0.8f; // 끝 두께
        lineRenderer.enabled = false;
        spaceBarCount = 0;
        isRunning = true;
        force = 0; //힘 초기화
    }

    void Update()
    {
        explosion.Stop();
        if (cannon.activeSelf) //SetActive(false)일때의 키보드 입력을 차단하기 위해
        {
            if (IsCannonTippedOver(cannon.transform))
            {
                Rigidbody cannonrb = cannon.GetComponent<Rigidbody>();
                if (cannonrb.velocity.magnitude <= 0.05f)
                {
                    cannon.transform.rotation = savedRotation;
                }
            }
            activeBall = gm.GetActiveBall();
            Rigidbody ballrb = activeBall.GetComponent<Rigidbody>();
            if (activeBall.name == "HamsterBall")
            {
                isGround = hamsterCollisionScript.onGround;
                isRespawn = hamsterCollisionScript.isWater;
                isGameOver = hamsterCollisionScript.gameOver;
            }
            else if (activeBall.name == "StickyBall")
            {
                stickyBallCollisionScript = activeBall.GetComponent<StickyBallCollision>();
                isGround = stickyBallCollisionScript.onGround;
                isRespawn = stickyBallCollisionScript.isWater;
                isGameOver = stickyBallCollisionScript.gameOver;
            }
            else
            {
                collisionScript = activeBall.GetComponent<CollisionDetection>();
                isGround = collisionScript.onGround;
                isRespawn = collisionScript.isWater;
                isGameOver = collisionScript.gameOver;
            }
            if (isGameOver)
            {
                transform.position = startPosition; //대포를 처음 시작 위치로 이동
                Time.timeScale = 0; //게임 일시정지
            }
            if (
                !isGameOver
                && spaceBarCount == 1
                && (isGround || isRespawn)
                && ballrb.velocity.magnitude <= 0.05f
            )
            {
                //공이 발사됐고 공이 땅에 닿아서 멈췄다면 다음 턴으로
                hamsterScript.enabled = false;
                hamsterCollisionScript.enabled = false;

                spacePressed = false;
                if (activeBall.name != "HamsterBall")
                {
                    if (activeBall.name == "StickyBall")
                    {
                        stickyBallCollisionScript = activeBall.GetComponent<StickyBallCollision>();
                        stickyBallCollisionScript.enabled = false;
                    }
                    else
                    {
                        collisionScript = activeBall.GetComponent<CollisionDetection>();
                        collisionScript.enabled = false;
                    }
                }

                StartCoroutine(Delay()); //1.5초 대기
                if (isRunning)
                {
                    spaceBarCount = 0;
                    gm.UpdateTurnUI();
                    gm.IncreaseTurn(); // 대포 위치 옮길 때마다 턴 수 증가, 리스폰 시 안옮겨도 턴 수 증단
                    if (isGround && !isRespawn)
                    {
                        cannon.transform.position += new Vector3(
                            activeBall.transform.position.x - prevBallPosition.x,
                            activeBall.transform.position.y + 2.5f,
                            activeBall.transform.position.z - prevBallPosition.z
                        ); //대포를 공의 전 턴의 마지막 위치 근처로 이동시킴

                        canon.transform.localRotation = initialLocalRotation; //포신을 초기 회전값으로 세팅
                        initialXRotation = canon.transform.localRotation.eulerAngles.x;
                        currentXRotation = initialXRotation;
                    }
                }
            }

            if (spaceBarCount == 0)
            {
                activeBall = gm.GetActiveBall();
                ballrb = activeBall.GetComponent<Rigidbody>();
                ballrb.useGravity = false;
                normal = GetSlopeNormal();
                activeBall.transform.position = firePoint.position;
                activeBall.transform.rotation = cannon.transform.rotation;
                activeBall.transform.Rotate(normal, 90f, Space.World);

                if (Input.GetAxis("Horizontal") != 0)
                {
                    float horizontalInput = Input.GetAxis("Horizontal"); //좌우 방향키 입력
                    //대포 전체 좌우 회전 조작(360도 회전 가능)
                    float rotationYChange = horizontalInput * rotationSpeed * Time.deltaTime;
                    Quaternion rotationChange = Quaternion.AngleAxis(rotationYChange, normal);
                    cannon.transform.rotation = rotationChange * cannon.transform.rotation;
                }

                if (Input.GetAxis("Vertical") != 0)
                {
                    float verticalInput = Input.GetAxis("Vertical"); //위아래 방향키 입력
                    //포신 위아래 회전 -> 발사각 조절
                    float rotationXChange = verticalInput * rotationSpeed * Time.deltaTime;
                    currentXRotation -= rotationXChange;

                    // 회전 각도 제한
                    currentXRotation = Mathf.Clamp(
                        currentXRotation,
                        initialXRotation - maxUp,
                        initialXRotation + maxDown
                    );
                    //로컬 회전
                    canon.transform.localRotation = Quaternion.Euler(currentXRotation, 0, 0);
                    //발사지점 회전 동기화
                    firePoint.rotation = canon.transform.rotation;
                }

                //maxForce = forceMultipler ;
                //forceIncreaseRate = forceMultipler;
                if (isForceIncreasing)
                {
                    force += Time.deltaTime * forceIncreaseRate;

                    if (force >= maxForce)
                    {
                        force = maxForce;
                        isForceIncreasing = false; // maxForce에 도달하면 감소로 전환
                    }
                }
                else
                {
                    force -= Time.deltaTime * forceIncreaseRate;
                    if (force <= minForce)
                    {
                        force = minForce;
                        isForceIncreasing = true; // minForce에 도달하면 증가로 전환
                    }
                }

                lineRenderer.enabled = true;
                lineRenderer.transform.position = firePoint.position; //firePoint위치로 lineRenderer시작점 이동

                if (activeBall.name == "BowlingBall")
                {
                    UpdateLineRenderer(
                        (firePoint.position - canon.transform.position) * force * ballrb.mass,
                        ballrb.mass
                    );
                }
                else
                {
                    UpdateLineRenderer(
                        (firePoint.position - canon.transform.position) * force,
                        ballrb.mass
                    );
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (spaceBarCount == 0)
                    {
                        gm.UpdateTurnUI(); //턴 수 UI 갱신
                    }

                    // space바를 누르면 공이 발사됨
                    // 공 발사 및 effect
                    activeBall = gm.GetActiveBall();
                    ballrb = activeBall.GetComponent<Rigidbody>();
                    prevBallPosition = activeBall.transform.position; //발사직전 공 위치 저장
                    savedRotation = cannon.transform.rotation; //발사 직전 대포의 회전값 저장
                    hamsterCollisionScript.enabled = true;
                    hamsterScript.enabled = true;
                    if (activeBall.name != "HamsterBall")
                    {
                        if (activeBall.name == "StickyBall")
                        {
                            stickyBallCollisionScript =
                                activeBall.GetComponent<StickyBallCollision>();
                            stickyBallCollisionScript.enabled = true;
                        }
                        else
                        {
                            collisionScript = activeBall.GetComponent<CollisionDetection>();
                            collisionScript.enabled = true;
                        }
                    }
                    lineRenderer.enabled = false;
                    explosion.transform.position = firePoint.position;
                    bigExplosion.SetActive(true);
                    explosion.Play();

                    if (activeBall.name == "BowlingBall")
                    {
                        ballrb.AddForce(
                            (firePoint.position - canon.transform.position) * force * ballrb.mass,
                            ForceMode.Impulse
                        );
                    }
                    else
                    {
                        ballrb.AddForce(
                            (firePoint.position - canon.transform.position) * force,
                            ForceMode.Impulse
                        );
                    }
                    ballrb.useGravity = true;
                    spaceBarCount++;
                    gm.DecreaseLifeLeft(); //발사 가능 횟수 감소
                    force = 0f; //힘 초기화
                    isForceIncreasing = true; //force가 0부터 시작하여 다시 증가하도록 조정
                    isRunning = false;
                    spacePressed = true;
                }
            }
        }
    }

    private float GetAngleBetweenVectorAndPlane(Vector3 vector, Vector3 normal)
    {
        // 벡터와 법선 사이의 각도 계산
        float dot = Vector3.Dot(vector, normal);
        float vectorMagnitude = vector.magnitude;
        float normalMagnitude = normal.magnitude;

        // 코사인 값 계산
        float cos = dot / (vectorMagnitude * normalMagnitude);

        // 라디안 값을 각도로 변환
        float angleWithNormal = Mathf.Acos(cos) * Mathf.Rad2Deg;

        return 90f - angleWithNormal;
    }

    private void UpdateLineRenderer(Vector3 initialVelocity, float mass)
    {
        float g = Physics.gravity.magnitude;
        float velocity = initialVelocity.magnitude / mass; //질량에 따른 예상 궤도 변화
        Vector3 unitVector = new Vector3(initialVelocity.x, 0, initialVelocity.z).normalized; //xz평면에 정사영 시킨 후 구한 단위 벡터
        float angle = GetAngleBetweenVectorAndPlane(initialVelocity, new Vector3(0, 1, 0));
        float timeStep = 0.1f;
        float fTime = 0f;
        for (int i = 0; i < N_TRAJECTORY_POINTS; i++)
        {
            float dw = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy =
                velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - (g * fTime * fTime * 0.5f);
            float dz = Vector3.Dot(dw * unitVector, new Vector3(0, 0, 1));
            float dx = Vector3.Dot(dw * unitVector, new Vector3(1, 0, 0));
            Vector3 pos = new Vector3(dx, dy, dz);
            lineRenderer.SetPosition(i, pos);
            fTime += timeStep;
        }
    }

    private bool IsCannonTippedOver(Transform cannonTransform)
    {
        Vector3 rotation = cannonTransform.eulerAngles;

        // 각도를 -180~180 범위로 정규화
        float normalizedX = rotation.x > 180 ? rotation.x - 360 : rotation.x;
        float normalizedZ = rotation.z > 180 ? rotation.z - 360 : rotation.z;

        // 임계값을 초과하면 넘어진 것으로 간주
        if (Mathf.Abs(normalizedX) > 70 || Mathf.Abs(normalizedZ) > 70)
        {
            return true; // 옆으로 넘어진 상태
        }

        return false; // 정상 상태
    }

    private Vector3 GetSlopeNormal()
    {
        // 경사면의 법선 벡터(normal vector) 계산
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            return hit.normal; //경사면일때
        }
        return Vector3.up; //평지일때
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1.5f); //1.5초 정도 대기
        // 기존에는 sticky ball에 대해서만 비활성화 처리를 했으나
        // 모래 지형 구현에 필요하여 모든 공에 대해 턴 시작 시 비활성화 처리
        Rigidbody ballrb = activeBall.GetComponent<Rigidbody>();
        ballrb.isKinematic = false;
        isRunning = true;
    }
}

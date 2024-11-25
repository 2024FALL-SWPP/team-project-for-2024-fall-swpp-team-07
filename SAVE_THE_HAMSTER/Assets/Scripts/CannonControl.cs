using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CannonControl : MonoBehaviour
{
    Stage1Manager gm;
    public GameObject cannon; // 대포 전체
    private GameObject activeBall; //발사할 공
    public Transform firePoint; // 탄환 발사 지점
    public LineRenderer lineRenderer;
    public ParticleSystem explosion;
    public GameObject bigExplosion;
    public GameObject armatureParent; //Hamster armature의 parent객체
    public GameObject hamsterBall; //햄스터 공 전체
    private BallControl hamsterScript; //hamster armature의 부모 객체인 gameObject에 적용되어 있는 script
    private CollisionDetection collisionScript;
    private const int N_TRAJECTORY_POINTS = 40;
    private float minForce = 0f; //최소 발사력
    private float maxForce; //최대 발사력
    private float forceMultipler = 5f;// 발사력에 적용할 비례 상수
    private float forceIncreaseRate;//발사력 증가 속도
    //cf) (최대 발사력 - 최소 발사력)/발사력 증가 속도 = 1 / fillSpeed (GaugeControl.cs)
    private float maxUp = 60f;
    private float maxDown = 10f;
    private float maxLeft = -120f;
    private float maxRight = 120f;

    private float scaleMultiplier = 2.5f; // 공 대비 대포 전체 비율-> 추후에 아이템으로 공이 커지는 효과를 구현한다면 대포도 커지게 설정
    
    private float initialYRotation;
    private float initialXRotation;
    private float currentYRotation;
    private float currentXRotation;

    private Quaternion initialLocalRotation;//canon의 localRotation값 저장

    private float force;

    //private int launchCount = 0;

    private bool isForceIncreasing = true;//force가 증가하고 있는지 여부

    public GameObject canon;//포신

    public float rotationSpeed = 100f; // 회전 속도

    public int spaceBarCount; //spaceBar가 눌린 횟수

 
    void Start()
    {
       gm = GameObject.Find("Stage1Manager").GetComponent<Stage1Manager>();
       hamsterScript = armatureParent.GetComponent<BallControl>();
       collisionScript = hamsterBall.GetComponent<CollisionDetection>();
       initialLocalRotation = canon.transform.localRotation;
       initialXRotation = canon.transform.eulerAngles.x;
       currentXRotation = initialXRotation;
       initialYRotation = cannon.transform.eulerAngles.y;
       currentYRotation = initialYRotation;
       lineRenderer.positionCount = N_TRAJECTORY_POINTS;
       // 선의 두께 설정
       lineRenderer.startWidth = 0.8f; // 시작 두께
       lineRenderer.endWidth = 0.8f; // 끝 두께
       lineRenderer.enabled = false;
       spaceBarCount = 0; 

    }
    

    void Update()
    {   
        explosion.Stop();
        if(cannon.activeSelf){ //SetActive(false)일때의 키보드 입력을 차단하기 위해
            activeBall = gm.GetActiveBall();
            Rigidbody ballrb = activeBall.GetComponent<Rigidbody>();

            if(spaceBarCount == 1 && collisionScript.onGround &&  ballrb.velocity.magnitude == 0f){
                //공이 발사됐고 공이 땅에 닿아서 멈췄다면 다음 턴으로
                hamsterScript.enabled = false;
                collisionScript.enabled = false;
                spaceBarCount = 0;
                cannon.transform.position = new Vector3(activeBall.transform.position.x, 2f, activeBall.transform.position.z); //대포를 공의 전 턴의 마지막 위치로 이동시킴
                canon.transform.localRotation = initialLocalRotation;//포신을 초기 회전값으로 세팅
                initialXRotation = canon.transform.eulerAngles.x;
                currentXRotation = initialXRotation;
                initialYRotation = cannon.transform.eulerAngles.y;
                currentYRotation = initialYRotation;
                
            }

            if(spaceBarCount == 0){

                ballrb.useGravity = false;
                activeBall.transform.position = firePoint.position;
                activeBall.transform.rotation = Quaternion.Euler(canon.transform.rotation.x, currentYRotation + 90f, canon.transform.rotation.z);

                if(Input.GetAxis("Horizontal") != 0){
                    float horizontalInput = Input.GetAxis("Horizontal"); //좌우 방향키 입력
                    //대포 전체 좌우 회전 조작
                    float rotationYChange = horizontalInput * rotationSpeed * Time.deltaTime;
                    currentYRotation += rotationYChange;

                    // 회전 각도 제한
                    currentYRotation = Mathf.Clamp(currentYRotation, initialYRotation + maxLeft, initialYRotation + maxRight);
                    cannon.transform.rotation = Quaternion.Euler(0, currentYRotation, 0);
                }

                if(Input.GetAxis("Vertical") != 0){
                    float verticalInput = Input.GetAxis("Vertical"); //위아래 방향키 입력
                    //포신 위아래 회전 -> 발사각 조절 
                    float rotationXChange = verticalInput * rotationSpeed * Time.deltaTime;
                    currentXRotation -= rotationXChange;

                    // 회전 각도 제한
                    currentXRotation = Mathf.Clamp(currentXRotation,  initialXRotation - maxUp,initialXRotation + maxDown);
                    canon.transform.rotation = Quaternion.Euler(currentXRotation,cannon.transform.eulerAngles.y,cannon.transform.eulerAngles.z);
                    firePoint.rotation = canon.transform.rotation;
                }

               
                maxForce = forceMultipler ;
                forceIncreaseRate = forceMultipler;
                if(isForceIncreasing){
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
                
                UpdateLineRenderer((firePoint.position - canon.transform.position) * force, ballrb.mass);
                
        

                
                if (Input.GetKeyDown(KeyCode.Space)){ 
                    // space바를 누르면 공이 발사됨
                    // 공 발사 및 effect
                    hamsterScript.enabled = true;
                    collisionScript.enabled = true;
                    lineRenderer.enabled = false;
                    explosion.transform.position = firePoint.position;
                    bigExplosion.SetActive(true);
                    explosion.Play();
                    
                    
                    ballrb.AddForce((firePoint.position - canon.transform.position) * force, ForceMode.Impulse);
                    ballrb.useGravity = true;
                    spaceBarCount++;
                    force = 0f; //힘 초기화
                } 

            }

            

        }
    
       
    }


    private void UpdateLineRenderer(Vector3 initialVelocity, float mass){
        float g = Physics.gravity.magnitude;
        float velocity = initialVelocity.magnitude / mass; //질량에 따른 예상 궤도 변화
        float angle = 300f - currentXRotation;
        float timeStep = 0.1f;
        float fTime = 0f;
        for(int i = 0 ; i < N_TRAJECTORY_POINTS; i++){
            float dw = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad) - ( g * fTime * fTime * 0.5f);
            float dz = dw * Mathf.Cos(Mathf.Abs(currentYRotation) * Mathf.Deg2Rad);
            float dx = dw * Mathf.Sin(Mathf.Abs(currentYRotation) * Mathf.Deg2Rad) * Mathf.Sign(currentYRotation);
            Vector3 pos = new Vector3(dx, dy, dz);
            lineRenderer.SetPosition(i, pos);
            fTime += timeStep;
        } 
    }
    

}

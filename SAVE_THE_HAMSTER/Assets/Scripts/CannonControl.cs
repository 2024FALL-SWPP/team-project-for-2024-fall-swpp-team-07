using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class CannonControl : MonoBehaviour
{
    public Transform cannonTransform; // 대포의 Transform
    public Transform firePoint; // 탄환 발사 지점
    public GameObject ball; // 발사할 공, ex.Hamster_m
    public LineRenderer lineRenderer;
    public ParticleSystem explosion;
    private const int N_TRAJECTORY_POINTS = 40;
    private float minForce = 0f; //최소 발사력
    private float maxForce = 20f; //최대 발사력
    private float forceIncreaseRate = 5f;//발사력 증가 속도
    private float minRotation = 230f; 
    private float maxRotation = 300f;
    private float maxLeft = -45f;
    private float maxRight = 45f;

    private float currentYRotation;
    private float currentXRotation;

    private float force;

    private bool isForceIncreasing = true;//force가 증가하고 있는지 여부

    public GameObject cannon;//포신

    public float rotationSpeed = 100f; // 회전 속도


    private bool spacePressed; //spaceBar가 눌렸는지 저장
 
    void Start()
    {
       currentYRotation = 0f;
       currentXRotation = 270f;
       cannonTransform.rotation = Quaternion.Euler(0,currentYRotation,0);
       cannon.transform.rotation = Quaternion.Euler(currentXRotation, 0, 0);
       lineRenderer.positionCount = N_TRAJECTORY_POINTS;
       lineRenderer.enabled = false;
       spacePressed = false;

    }
    

    void Update()
    {   
        explosion.Stop();
        
        if(!spacePressed){

        //대포 전체 좌우 회전 조작
        float horizontalInput = Input.GetAxis("Horizontal"); //좌우 방향키 입력
        cannonTransform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
        float rotationYChange = Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime;
        currentYRotation += rotationYChange;

        // 회전 각도 제한
        currentYRotation = Mathf.Clamp(currentYRotation, maxLeft, maxRight);
        cannonTransform.rotation = Quaternion.Euler(0, currentYRotation, 0);

        //포신 위아래 회전 -> 발사각 조절 
        float rotationChange = Input.GetAxis("Vertical") * rotationSpeed * Time.deltaTime;
        currentXRotation -= rotationChange;

        // 회전 각도 제한
        currentXRotation = Mathf.Clamp(currentXRotation, minRotation, maxRotation);
        cannon.transform.rotation = Quaternion.Euler(currentXRotation,cannonTransform.eulerAngles.y,cannonTransform.eulerAngles.z);
        firePoint.rotation = cannon.transform.rotation;
        
        }
        
        //공 발사
        if (Input.GetKey(KeyCode.Space)) // 스페이스바를 누르고 있는 동안 force가 min, max사이에서 진동
        {
            spacePressed = true;
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
            lineRenderer.transform.position = firePoint.position;//firePoint위치로 lineRenderer시작점 이동
            UpdateLineRenderer((firePoint.position - cannon.transform.position) * force);
        } 

        else if (Input.GetKeyUp(KeyCode.Space)) // 스페이스바를 떼면
        {
            // 탄환 생성 및 발사
            lineRenderer.enabled = false;
            explosion.transform.position = firePoint.position;
            explosion.Play();
            GameObject projectile = Instantiate(ball, firePoint.position, firePoint.rotation);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce((firePoint.position - cannon.transform.position) * force, ForceMode.Impulse);
            force = 0f;//force값 초기화
        }

        
    
       
    }


    private void UpdateLineRenderer(Vector3 initialVelocity){
        float g = Physics.gravity.magnitude;
        float velocity = initialVelocity.magnitude * 0.5f;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterAnimationControl : MonoBehaviour
{
    // Start is called before the first frame update
    
    // Stage1Manager 혹은 GaolManager 불러오기
    // private Stage1Manager stage1Manager;
    // private GoalManager goalManager;
    public GameObject hamsterBall;//전체 햄스터 공
    public GameObject hamster;//armature

    public GameObject ball; // 게임 성공 시 종료 애니메이션 시 충돌 감지
    public GameObject ground; // 게임 성공 시 종료 애니메이션 시 충돌 감지
    public PhysicMaterial bouncyMaterial; // 게임 성공 시 종료 애니메이션 용
    public PhysicMaterial regularMaterial; // 기존 머티리얼로 변경 용
    public PhysicMaterial groundMaterial; // 기존 머티리얼 변경 용
    private Collider ballCollider;
    private Collider groundCollider;

    private Animator animator;
    private Rigidbody rb;
    private bool success = false; // 성공 여부 Stage1Manager 혹은 GaolManager 연결하여 가져오기
    public float rotationSpeed = 1000f; // 게임 성공 시 종료 애니메이션 용
    private bool successAnimationPlayed = false; // 게임 성공 시 종료 애니메이션 플레이 여부
    private bool spinjumped = false; // 햄스터 공 튕기는 애니메이션 플레이 여부
    private float ceremonyTime = 0f; // 게임 성공 시 종료 애니메이션 시간체크용
    //private int ceremonyCount = 0; // 게임 성공 시 종료 애니메이션 횟수체크용

    void Start()
    {
        animator = hamsterBall.GetComponent<Animator>();
        rb = hamsterBall.GetComponent<Rigidbody>();
        ballCollider = ball.GetComponent<Collider>();
        groundCollider = ground.GetComponent<Collider>();
        success = false;
        successAnimationPlayed = false;
        ceremonyTime = 0f;
        // Stage1Manager 혹은 GaolManager 불러오기
        // stage1Manager = FindObjectOfType<Stage1Manager>();
        // success = stage1Manager.success;
        // goalManager = FindObjectOfType<GoalManager>();
        // success = goalManager.success;
    }

    // Update is called once per frame
    void Update()
    {
        if (!successAnimationPlayed && success)
        {
            // 게임 성공 시 종료 애니메이션
            // 햄스터 뛰기 애니메이션
            Debug.Log("success");
            animator.SetInteger("Speed", 2);
            ballCollider.material = bouncyMaterial;
            groundCollider.material = bouncyMaterial;
            if (!spinjumped)
            { 
                spinjumpAnimationPlayer();
                spinjumped = true;
            }
            ceremonyTime += Time.deltaTime;
            if (ceremonyTime >= 10f)
            {
                successAnimationPlayed = true;
                animator.SetTrigger("Success_Trig");
                ballCollider.material = regularMaterial;
                groundCollider.material = groundMaterial;
            }
        }
        // else if (!success)
        // {
        //     float speed = rb.velocity.magnitude; // 공의 속도 계산
        //     Debug.Log(speed);
        //     // 속도에 따라 애니메이션 상태 전환
        //     if (speed < 1f && speed > 0.1f)
        //     {
        //         animator.SetInteger("Speed", 1);
        //     }
        //     else if (speed >= 1f)
        //     {
        //         animator.SetInteger("Speed", 2);
        //     }
        //     else
        //     {
        //         animator.SetTrigger("Idle_Trig");
        //     }
        // }
    }

    private void spinjumpAnimationPlayer()
    {
        // 햄스터 공 회전 애니메이션 효과
        rb.AddTorque(Vector3.up * rotationSpeed, ForceMode.Impulse);
        // 햄스터 공 튕기는 애니메이션 효과
        // 1초간 공중에 있도록 점프
        rb.AddForce(Vector3.up * Physics.gravity.magnitude * 0.5f, ForceMode.VelocityChange);
    }

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.CompareTag("Ground") && success && ceremonyCount < 5)
    //     {
    //         spinjumpAnimationPlayer();
    //         ceremonyCount ++;
    //     }
    // }
}

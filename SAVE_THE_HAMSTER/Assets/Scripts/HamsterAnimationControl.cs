using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterAnimationControl : MonoBehaviour
{
    // Start is called before the first frame update

    // Stage1Manager 혹은 GaolManager 불러오기
    // private Stage1Manager stage1Manager;
    // private GoalManager goalManager;
    public GameObject hamsterBall; //전체 햄스터 공

    // public GameObject hamster;//armature

    public GameObject ball; // 게임 성공 시 종료 애니메이션 시 충돌 감지
    public GameObject ground; // 게임 성공 시 종료 애니메이션 시 충돌 감지
    public PhysicMaterial bouncyMaterial; // 게임 성공 시 종료 애니메이션 용
    public PhysicMaterial regularMaterial; // 기존 머티리얼로 변경 용
    public PhysicMaterial groundMaterial; // 기존 머티리얼 변경 용
    public GameObject successParticle; //성공 시 활성화
    private Collider ballCollider;
    private Collider groundCollider;

    private Animator animator;
    private Rigidbody rb;

    private bool enableMiniMove = false; // 햄스터 공 발사 후 miniMove 여부

    private bool success = false; // 성공 여부 StageManager에서 set해줌
    private float rotationSpeed = 3000f; // 게임 성공 시 종료 애니메이션 용
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!successAnimationPlayed && success)
        {
            // 게임 성공 시 종료 애니메이션
            // 햄스터 뛰기 애니메이션
            // Debug.Log("success");
            animator.SetInteger("Speed", 2);
            ballCollider.material = bouncyMaterial;
            groundCollider.material = bouncyMaterial;
            successParticle.SetActive(true);
            if (!spinjumped)
            {
                spinjumpAnimationPlayer();
                spinjumped = true;
            }
            ceremonyTime += Time.deltaTime;
            if (ceremonyTime >= 10f)
            {
                successAnimationPlayed = true;
                ballCollider.material = regularMaterial;
                groundCollider.material = groundMaterial;
            }
        }

        if (enableMiniMove)
        {
            if (true)
            {
                miniMove();
            }
        }
    }

    // 성공 시 사용
    private void spinjumpAnimationPlayer()
    {
        // 햄스터 공 회전 애니메이션 효과
        rb.AddTorque(Vector3.up * rotationSpeed, ForceMode.Impulse);
        // 햄스터 공 튕기는 애니메이션 효과
        // 1초간 공중에 있도록 점프
        rb.AddForce(Vector3.up * Physics.gravity.magnitude * 0.5f, ForceMode.VelocityChange);
    }

    private void miniMove() { }

    public void SetSuccess()
    {
        success = true;
    }
}

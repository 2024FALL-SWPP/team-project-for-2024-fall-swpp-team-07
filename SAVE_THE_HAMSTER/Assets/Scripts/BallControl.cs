using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private Vector3 rotationDirection; // 공의 회전 방향
    public float rotationSpeed = 100f; // 공의 회전 속도
    public GameObject ball; //햄스터를 둘러싸는 공
    public GameObject hamsterBall; //전체 햄스터공
    public GameObject firePoint; //포구
    public GameObject cannon; //대포 전체
    private Animator animator; // 햄스터의 Animator
    private Quaternion originalRotation;

    public Vector3 offsetDirection;

    void Start() { }

    void OnEnable()
    {
        originalRotation = hamsterBall.transform.rotation; //발사 직전 hamsterBall의 rotation값 저장
        animator = hamsterBall.GetComponent<Animator>();
        rb = hamsterBall.GetComponent<Rigidbody>();
        SetRandomRotationDirection(); //매 발사(매 공)마다 회전방향이 무작위로 달라짐
    }

    // Update is called once per frame
    void Update()
    {
        offsetDirection =
            new Vector3(firePoint.transform.position.x, 0, firePoint.transform.position.z)
            - new Vector3(cannon.transform.position.x, 0, cannon.transform.position.z);
        transform.rotation = originalRotation;
        transform.position = ball.transform.position - offsetDirection.normalized * 0.55f;
        float speed = rb.velocity.magnitude; // 공의 속도 계산
        // 속도에 따라 애니메이션 상태 전환
        if (speed < 10f && speed > 2f)
        {
            animator.SetInteger("Speed", 1);
        }
        else if (speed >= 10f)
        {
            animator.SetInteger("Speed", 2);
        }
        else
        {
            animator.SetTrigger("Idle_Trig");
        }
    }

    void FixedUpdate()
    {
        // 공을 계속 회전시키기
        // Debug.Log(hamsterBall.GetComponent<CollisionDetection>().onGround);
        // if(!hamsterBall.GetComponent<CollisionDetection>().onGround){
        //     hamsterBall.transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
        // }
        // Debug.Log(hamsterBall.GetComponent<CollisionDetection>().onGround);
        if (!hamsterBall.GetComponent<HamsterCollision>().onGround)
        {
            hamsterBall.transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
        }
    }

    private void SetRandomRotationDirection()
    {
        // 랜덤한 회전 방향 생성
        float randomX = Random.Range(-1f, 1f);
        float randomY = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        rotationDirection = new Vector3(randomX, randomY, randomZ).normalized; // 정규화
    }
}

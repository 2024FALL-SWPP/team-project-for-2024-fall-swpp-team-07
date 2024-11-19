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
    private bool onGround;
    public float deceleration = 0.1f; // 공 감속 비율
    public GameObject ball; //햄스터를 둘러싸는 공 
    public GameObject hamsterBall; //전체 햄스터공
    private Animator animator; // 햄스터의 Animator

    private Quaternion originalRotation;

    void Start()
    {   
        hamsterBall.transform.rotation = Quaternion.Euler(0f,90f,0f);
        originalRotation =  Quaternion.Euler(0f,90f,0f); 
        animator = hamsterBall.GetComponent<Animator>();
        rb = hamsterBall.GetComponent<Rigidbody>();
        SetRandomRotationDirection();//매 발사(매 공)마다 회전방향이 무작위로 달라짐
        onGround = false;

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = originalRotation ; 
        transform.position = ball.transform.position + new Vector3(0f,0f,-0.7f);
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
        if(!onGround){
            hamsterBall.transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
    }
    void OnCollisionStay(Collision collision)
    {
        // 공이 충돌 중일 때 속도를 감소시킴
        if (rb != null)
        {
            // 현재 속도에 감속 비율을 곱하여 속도를 줄임
            rb.velocity = rb.velocity * (1f - deceleration);
            
            // 속도가 매우 작은 경우에는 속도를 0으로 설정
            if (rb.velocity.magnitude < 0.05f)
            {
                rb.velocity = Vector3.zero;//속도 멈추기
                rb.angularVelocity = Vector3.zero;//회전 멈추기
            }
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

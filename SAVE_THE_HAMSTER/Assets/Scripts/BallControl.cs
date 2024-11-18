using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody rb;
    private Vector3 rotationDirection; // 공의 회전 방향
    public float rotationSpeed = 100f; // 공의 회전 속도

    private bool onGround = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetRandomRotationDirection();//매 발사(매 공)마다 회전방향이 무작위로 달라짐
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        // 공을 계속 회전시키기
        if(!onGround){
        transform.Rotate(rotationDirection * rotationSpeed * Time.deltaTime);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
            StartCoroutine(StopBallAfterDelay(5f)); // 5초 후 멈추기
        }
    }

    private IEnumerator StopBallAfterDelay(float delay)
    {
        // 일정 시간 대기
        yield return new WaitForSeconds(delay);

        // 공 멈추기
        rb.velocity = Vector3.zero; // 속도를 0으로 설정
        rb.angularVelocity = Vector3.zero; // 회전 속도를 0으로 설정
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

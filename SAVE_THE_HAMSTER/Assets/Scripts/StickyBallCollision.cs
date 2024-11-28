using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBallCollision : CollisionDetection
{
    // private Rigidbody rb;

    public GameObject cannon;
    // Start is called before the first frame update
    // void Start()
    // {
    //     rb = GetComponent<Rigidbody>();
    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    protected override void childOnCollisionEnter(Collision collision)
    {
        if (cannon.activeSelf && collision.gameObject.CompareTag("Ground"))
        {
            StickToCollisionPoint(collision);
        }
    }

    void StickToCollisionPoint(Collision collision)
    {
        // 충돌 지점 가져오기
        ContactPoint contact = collision.contacts[0]; //현재 발생한 충돌의 첫번째 접촉점
        Vector3 collisionPoint = contact.point;

        // 오브젝트를 충돌 지점으로 이동
        transform.position = collisionPoint;

        // Rigidbody 비활성화
        rb.isKinematic = true;

        // 속도와 각속도 0으로 설정
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }


}

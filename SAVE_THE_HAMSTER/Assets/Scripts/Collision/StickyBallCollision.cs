using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBallCollision : CollisionDetection
{
    public GameObject cannon;
    private bool sticky = true;

    protected override void childOnCollisionEnter(Collision collision)
    {
        if (sticky) Debug.Log("Still Sticky");
        else Debug.Log("Not Sticky");
        if (cannon.activeSelf && collision.gameObject.CompareTag("Ground"))
        {
            if(sticky) // sticky 기능 있는 경우 첫번째 충돌 위치에 정지
            {
                StickToCollisionPoint(collision);
            }
        }

        if (cannon.activeSelf && collision.gameObject.CompareTag("Sand")) // 2 구현
        {
            if(sticky) // sticky 기능 있는 경우 모래지형에 대해서도 첫번째 충돌 위치에 정지
            {
                StickToCollisionPoint(collision);
            }
            Debug.Log("Sticky lost");
            sticky = false; // 모래지형에 한 번이라도 충돌 시 sticky 기능 상실
            // else의 경우 CollisionDetection.cs에서 모든 공에 해당하는 부분 따르니 구현 필요없음
        }
    }

    // void StickToCollisionPoint(Collision collision)
    // {
    //     // 충돌 지점 가져오기
    //     ContactPoint contact = collision.contacts[0]; //현재 발생한 충돌의 첫번째 접촉점
    //     Vector3 collisionPoint = contact.point;

    //     // 오브젝트를 충돌 지점으로 이동
    //     transform.position = collisionPoint;

    //     // Rigidbody 비활성화
    //     rb.isKinematic = true;

    //     // 속도와 각속도 0으로 설정
    //     rb.velocity = Vector3.zero;
    //     rb.angularVelocity = Vector3.zero;
    // }
}

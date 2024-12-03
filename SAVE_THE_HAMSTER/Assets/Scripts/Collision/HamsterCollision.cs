using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCollision : CollisionDetection
{
    public float deceleration = 0.1f; // 공 감속 비율

    void OnCollisionStay(Collision collision)
    {
        // 공이 충돌 중일 때 속도를 감소시킴
        if (rb != null)
        {
            // 현재 속도에 감속 비율을 곱하여 속도를 줄임
            rb.velocity = rb.velocity * (1f - deceleration);

            // 속도가 작은 경우에는 속도를 0으로 설정
            if (rb.velocity.magnitude < 0.05f)
            {
                rb.velocity = Vector3.zero; //속도 멈추기
                rb.angularVelocity = Vector3.zero; //회전 멈추기
            }
        }
    }
}

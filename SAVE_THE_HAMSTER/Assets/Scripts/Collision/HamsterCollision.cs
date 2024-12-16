using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCollision : CollisionDetection
{
    public float deceleration = 0.1f; // 공 감속 비율
    public bool isSteep; //충돌 지면이 가파른지
    private float CalculateAngle(Vector3 normal, Vector3 ground)
    {
        // 두 벡터를 정규화 
        Vector3 normalizedNormal = normal.normalized;
        Vector3 normalizedGround = ground.normalized;
        float dotProduct = Vector3.Dot(normalizedNormal, normalizedGround);
        float angleInRadian = Mathf.Acos(dotProduct);
        // 라디안을 도 단위로 변환
        float angleInDegree = angleInRadian * Mathf.Rad2Deg;
        return angleInDegree;
    }

    void OnCollisionStay(Collision collision)
    {
        
        // 공이 충돌 중일 때 속도를 감소시킴
        float angle = CalculateAngle(collision.contacts[0].normal, new Vector3(0,1,0));
        if(angle < 10){
            isSteep = false;
        }
        else{
            isSteep = true;
        }
        if (rb != null && !isSteep)
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
    protected override void OnEnable()
    {
        base.OnEnable(); //부모 클래스 OnEnable호출
    }
}

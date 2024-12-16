using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBallCollision : CollisionDetection
{
    public GameObject cannon;
    private bool sticky = true;

    protected override void childOnCollisionEnter(Collision collision)
    {
        if (sticky)
            Debug.Log("Still Sticky");
        else
            Debug.Log("Not Sticky");
        if (
            cannon.activeSelf
            && collision.gameObject.CompareTag("Ground")
            && cannonControl.spaceBarCount == 1
        )
        {
            if (sticky) // sticky 기능 있는 경우 첫번째 충돌 위치에 정지
            {
                StickToCollisionPoint(collision);
            }
        }

        if (cannon.activeSelf && collision.gameObject.CompareTag("Sand")) // 2 구현
        {
            if (sticky) // sticky 기능 있는 경우 모래지형에 대해서도 첫번째 충돌 위치에 정지
            {
                StickToCollisionPoint(collision);
            }
            Debug.Log("Sticky lost");
            sticky = false; // 모래지형에 한 번이라도 충돌 시 sticky 기능 상실
            // else의 경우 CollisionDetection.cs에서 모든 공에 해당하는 부분 따르니 구현 필요없음
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable(); //부모 클래스 OnEnable호출
    }
}

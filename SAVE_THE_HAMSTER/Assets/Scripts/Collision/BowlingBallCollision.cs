using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowlingBallCollision : CollisionDetection
{
    public GameObject cannon;

    protected override void childOnCollisionEnter(Collision collision)
    {
        if (
            cannon.activeSelf
            && collision.gameObject.CompareTag("Sand")
            && cannonControl.spaceBarCount == 1
        )
        {
            Debug.Log("bowling ball on sand");
            StickToCollisionPoint(collision);
        }
    }
}

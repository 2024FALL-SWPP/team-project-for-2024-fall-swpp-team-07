using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZoneBehavior : MonoBehaviour
{
    Vector3 windDirection = Vector3.forward;
    float windForce = 5.0f;

    Rigidbody ballRb;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            windDirection = -transform.forward;
            ballRb = other.attachedRigidbody;
            ballRb.AddForce(windDirection * windForce, ForceMode.Force);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFlow : MonoBehaviour
{
    public Vector3 localFlowDirection = Vector3.forward;
    public float flowSpeed = 5f;

    Rigidbody ballRb;

    public Vector3 GetFlowForce()
    {
        // 월드 좌표계에서의 물살 힘을 반환
        return transform.TransformDirection(localFlowDirection.normalized) * flowSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ballRb = other.GetComponent<Rigidbody>();
            if (ballRb == null)
            {
                ballRb = other.GetComponentInParent<Rigidbody>();
            }

            ballRb.velocity *= 0.01f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ballRb = other.GetComponent<Rigidbody>();
            if (ballRb == null)
            {
                ballRb = other.GetComponentInParent<Rigidbody>();
            }
            ballRb.AddForce(GetFlowForce(), ForceMode.Acceleration);
        }
    }
}

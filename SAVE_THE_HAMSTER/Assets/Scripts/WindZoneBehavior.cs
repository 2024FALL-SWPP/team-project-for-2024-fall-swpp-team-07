using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZoneBehavior : MonoBehaviour
{
    Vector3 windDirection = Vector3.forward;
    public float windForce = 10.0f;

    Rigidbody ballRb;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            windDirection = transform.forward;
            ballRb = other.attachedRigidbody;
            ballRb.AddForce(windDirection * windForce, ForceMode.Force);
        }
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }
}

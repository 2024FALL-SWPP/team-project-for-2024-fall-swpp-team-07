using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalKeyRotation : MonoBehaviour
{
    // Start is called before the first frame update
    public float rotateSpeed = 40.0f;
    Vector3 rotateAngle = Vector3.up;

    void Start() { }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotateAngle * Time.deltaTime * rotateSpeed);
    }
}

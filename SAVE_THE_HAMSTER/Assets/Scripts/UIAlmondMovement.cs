using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAlmondMovement : MonoBehaviour
{
    public float rotateSpeed = 40.0f;
    Vector3 rotateAngle = Vector3.forward;
    int sign = 1;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (transform.rotation.eulerAngles.z > 210)
        {
            sign = -1;
        }
        else if (transform.rotation.eulerAngles.z < 150)
        {
            sign = 1;
        }
        transform.Rotate(rotateAngle * Time.deltaTime * rotateSpeed * sign);
    }
}

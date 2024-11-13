using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectSceneCameraController : MonoBehaviour
{
    private float mouseScrollValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseScrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollValue < 0 && transform.position.x < 1.3f) // move camera to right when scrolling down
        {
            transform.Translate(Vector3.right * mouseScrollValue);
        }
        else if (mouseScrollValue > 0 && transform.position.x > -1.3f) // move camera to left when scrolling up
        {
            transform.Translate(Vector3.right * mouseScrollValue);
        }
    }
}

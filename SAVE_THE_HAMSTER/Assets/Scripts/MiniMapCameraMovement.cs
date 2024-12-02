using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraMovement : MonoBehaviour
{
    // public GameObject[] balls; => applied design pattern
    GameObject activeBall;
    GameObject cannon;
    StageManager gm;
    Vector3 _position;
    Quaternion _rotation;

    CannonControl cannonControl;

    // public GameObject quad;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<StageManager>();
        cannon = gm.cannon;
        cannonControl = cannon.GetComponent<CannonControl>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // Applied design pattern
        /*
        foreach (GameObject ball in balls)
        {
            if (ball.activeSelf)
            {
                activeBall = ball;
                break;
            }
        }
        */


        if (cannon.activeSelf && cannonControl.spaceBarCount == 0)
        {
            _position = new Vector3(
                cannon.transform.position.x,
                transform.position.y,
                cannon.transform.position.z
            );

            _rotation = Quaternion.Euler(0, cannon.transform.rotation.eulerAngles.y - 135, 0);
            // Debug.Log(cannon.transform.rotation.y);
        }
        else
        {
            activeBall = gm.GetActiveBall();
            _position = new Vector3(
                activeBall.transform.position.x,
                transform.position.y,
                activeBall.transform.position.z
            );
            _rotation = transform.rotation;
        }

        transform.SetPositionAndRotation(_position, _rotation);
    }
}

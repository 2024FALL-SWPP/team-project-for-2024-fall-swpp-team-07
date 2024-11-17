using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCameraMovement : MonoBehaviour
{
    // public GameObject[] balls; => applied design pattern
    GameObject activeBall;
    Stage1Manager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Stage1Manager").GetComponent<Stage1Manager>();
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
        activeBall = gm.GetActiveBall();
        transform.position = new Vector3(
            activeBall.transform.position.x,
            transform.position.y,
            activeBall.transform.position.z
        );
    }
}

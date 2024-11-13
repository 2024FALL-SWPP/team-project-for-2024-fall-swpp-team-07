using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTransition : MonoBehaviour
{
    // USE THIS WHEN CHANGING BALLS

    private int ballType; // current ball type
    public GameObject[] balls;
    Vector3[] ballOffsets;

    /* manual offsets (FOR REFERENCE)
    Vector3 hamsterOffset = Vector3.zero;
    Vector3 stickyOffset = new Vector3(-0.518000007, 0.143000007, 0);
    Vector3 bowlingOffset = new Vector3(-0.51700002, 0.150000006, -0.0209999997);
    Vector3 bouncyOffset = new Vector3(-0.518999994, 0.156000003, 0);
    Vector3 footballOffset = new Vector3(-0.48300001, 0.133000001, 0);
    */

    public void ChangeBall(int newBall)
    {
        balls[ballType].SetActive(false);
        gameObject.transform.Translate(
            balls[ballType].transform.localPosition - ballOffsets[ballType]
        );
        balls[ballType].transform.localPosition = ballOffsets[ballType];
        balls[newBall].SetActive(true);
        ballType = newBall;
    }

    // Start is called before the first frame update
    void Start()
    {
        ballOffsets = new Vector3[balls.Length];
        // initialize offset
        for (int i = 0; i < balls.Length; i++)
        {
            ballOffsets[i] = balls[i].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /* Testing Purpose */
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            int newBall = (ballType + 1) % balls.Length;
            ChangeBall(newBall);
        }
    }
}

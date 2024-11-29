using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallTransition : MonoBehaviour
{
    // USE THIS WHEN CHANGING BALLS

    public int ballType; // current ball type
    public GameObject[] balls;
    Vector3[] ballOffsets;
    private bool blockLeftShift = false;
    private bool blockRightShift = false;
    public GameObject cannon; //대포 전체
    CannonControl cannonControl;

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
        cannonControl = cannon.GetComponent<CannonControl>();
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
        // 공이 날아가는 중에는 Ball transition 제한
        if (cannonControl.spaceBarCount == 1)
        {
            blockLeftShift = true;
            blockRightShift = true;
        }
        else if (cannonControl.spaceBarCount == 0)
        {
            blockLeftShift = false;
            blockRightShift = false;
        }

        //즉, 공의 종류를 먼저 선택 -> 방향키 발사각 조절 -> space바 발사세기 조절
        if (
            !blockLeftShift
            && !blockRightShift
            && (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift))
        )
        {
            int newBall = (ballType + 1) % balls.Length;
            ChangeBall(newBall);
        }
    }
}

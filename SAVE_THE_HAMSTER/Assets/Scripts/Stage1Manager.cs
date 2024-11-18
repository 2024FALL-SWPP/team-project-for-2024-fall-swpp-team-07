using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : MonoBehaviour
{
    // This script takes care of all necessary info for stage 1.

    public int totalLife;
    int lifeLeft;
    int totalAlmond = 1;
    bool[] almondStatus;

    bool _timerActive = false;
    float _currentTime = 0;

    public GameObject[] balls;
    public GameObject animationCamera;
    GameObject canvas;

    public GameObject GetActiveBall()
    {
        foreach (GameObject ball in balls)
        {
            if (ball.activeSelf)
            {
                return ball;
            }
        }
        return null;
    }

    public void AnimationEnd(int animation)
    {
        // Starting Animation
        if (animation == 1)
        {
            StartGame();
        }

        if (animation == 2)
        {
            EndGame();
        }
    }

    void StartGame()
    {
        animationCamera.SetActive(false);
        canvas.SetActive(true);
        _timerActive = true;
    }

    void EndGame()
    {
        // somehow return/give [almondStatus, # of fires (totalLife - lifeLeft), elapsed time].
    }

    public void GetAlmond(int almondNumber)
    {
        almondStatus[almondNumber] = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        lifeLeft = totalLife;
        canvas = GameObject.Find("Canvas");
        canvas.SetActive(false);
        almondStatus = new bool[totalAlmond]; // initializes to false. // TODO: change on enter by getting an array of almondStatus.
        _timerActive = false;
        _currentTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerActive)
        {
            _currentTime += Time.deltaTime;
        }
    }
}

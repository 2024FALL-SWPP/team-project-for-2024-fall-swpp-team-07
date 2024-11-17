using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1Manager : MonoBehaviour
{
    // This script takes care of all necessary info for stage 1.

    public int totalLife;
    int lifeLeft;

    public GameObject[] balls;

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

    // Start is called before the first frame update
    void Start()
    {
        lifeLeft = totalLife;
    }

    // Update is called once per frame
    void Update() { }
}

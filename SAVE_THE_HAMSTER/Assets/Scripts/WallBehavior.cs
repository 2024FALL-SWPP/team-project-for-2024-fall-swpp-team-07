using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallBehavior : MonoBehaviour
{
    Stage1Manager gm;

    // public GameObject hardWall;
    // public GameObject softWall;

    GameObject currentBall;
    public GameObject[] breakableBalls;

    // public GameObject[] chunks;
    public bool isBreakable = false;

    /*
    void Hardify()
    {
        softWall.SetActive(false);
        hardWall.SetActive(true);
    }

    void Softify()
    {
        hardWall.SetActive(false);
        softWall.SetActive(true);
    }
    */

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Stage1Manager").GetComponent<Stage1Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        currentBall = gm.GetActiveBall();
        if (breakableBalls.Contains(currentBall))
        {
            isBreakable = true;
            /*
            foreach (GameObject chunk in chunks)
            {
                Rigidbody rb = chunk.GetComponent<Rigidbody>();
                
                rb.isKinematic = false;
            }
            Softify();
            */
        }
        else
        {
            isBreakable = false;
            /*
            foreach (GameObject chunk in chunks)
            {
                Rigidbody rb = chunk.GetComponent<Rigidbody>();
                rb.isKinematic = true;
            }
            // Hardify();
            */
        }
    }
}

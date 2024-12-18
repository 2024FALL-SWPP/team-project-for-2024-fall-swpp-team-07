using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterController : MonoBehaviour
{
    public GameObject glassBall;
    public int level;
    private Vector3 offset = new Vector3();

    // Start is called before the first frame update
    void Start()
    {
        if (level == 1)
        {
            offset = new Vector3(0.04f, -0.22f, -0.06f);
        }
        else if (level == 2)
        {
            offset = new Vector3(-0.24f, -0.18f, 0f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = glassBall.transform.position + offset;
    }
}

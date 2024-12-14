using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBehavior : MonoBehaviour
{
    public float leftEndX = -5.4f;
    public float rightEndX = 16f;

    public int direction = 1;

    public float speed = 2f;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        if (transform.localPosition.x < leftEndX)
        {
            transform.localPosition = new Vector3(
                rightEndX,
                transform.localPosition.y,
                transform.localPosition.z
            );
        }

        if (transform.localPosition.x > rightEndX)
        {
            transform.localPosition = new Vector3(
                leftEndX,
                transform.localPosition.y,
                transform.localPosition.z
            );
        }

        transform.Translate(transform.right * direction * speed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsTester : MonoBehaviour
{
    private float forceMagnitude; // 힘의 세기를 조절할 수 있는 변수
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        forceMagnitude = rb.mass * 8.0f;
    }

    // Update is called once per frame
    void Update()
    {
        // 스페이스바를 누르면 힘을 가함
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 45도 방향으로 힘을 가함 (x와 y를 1:1 비율로 설정)
            Vector3 forceDirection = new Vector3(1f, 1f, 0f).normalized;
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // 수평 방향으로 힘을 가함
            Vector3 forceDirection = new Vector3(1f, 0f, 0f);
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // 수평 방향으로 힘을 가함
            Vector3 forceDirection = new Vector3(-1f, 0f, 0f);
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // 수평 방향으로 힘을 가함
            Vector3 forceDirection = new Vector3(0f, 0f, -1f);
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // 수평 방향으로 힘을 가함
            Vector3 forceDirection = new Vector3(0f, 0f, 1f);
            rb.AddForce(forceDirection * forceMagnitude, ForceMode.Impulse);
        }
    }
}

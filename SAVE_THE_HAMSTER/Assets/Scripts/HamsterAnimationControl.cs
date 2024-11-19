using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterAnimationControl : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject hamsterBall;//전체 햄스터 공
    public GameObject hamster;//armature

    private Animator animator;
    private Rigidbody rb;

    void Start()
    {
        animator = hamster.GetComponent<Animator>();
        rb = hamsterBall.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = rb.velocity.magnitude; // 공의 속도 계산
        // 속도에 따라 애니메이션 상태 전환
        if (speed < 1f && speed > 0.1f)
        {
            animator.SetInteger("Speed", 1);
        }
        else if (speed >= 1f)
        {
            animator.SetInteger("Speed", 2);
        }
        else
        {
            animator.SetTrigger("Idle_Trig");
        }
    }
}

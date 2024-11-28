using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HamsterCollision : MonoBehaviour
{
    // Start is called before the first frame update
    
    Stage1Manager gm;
    private Rigidbody rb;
    public bool isWater = false;
    public bool onGround = false;
    public float deceleration = 0.1f; // 공 감속 비율
    public bool gameOver = false;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.Find("Stage1Manager").GetComponent<Stage1Manager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        onGround = false;
        isWater = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }
        if(collision.gameObject.CompareTag("Respawn"))
        {
            isWater = true;
            gm.minusLifeLeft(); //발사 가능 횟수 추가 감소 (벌타)
            // gm.lifeLeft--; //기존
        }
        if(collision.gameObject.CompareTag("Goal"))
        {
            gm.AnimationEnd(2);
        }
        if(collision.gameObject.CompareTag("Death")){
            //원래 시작 위치로 돌아감
            gameOver = true;
        }

    }
    void OnCollisionStay(Collision collision)
    {
        // 공이 충돌 중일 때 속도를 감소시킴
        if (rb != null)
        {
            // 현재 속도에 감속 비율을 곱하여 속도를 줄임
            rb.velocity = rb.velocity * (1f - deceleration);
            
            // 속도가 작은 경우에는 속도를 0으로 설정
            if (rb.velocity.magnitude < 0.05f)
            {
                rb.velocity = Vector3.zero;//속도 멈추기
                rb.angularVelocity = Vector3.zero;//회전 멈추기
            }
        }
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public bool onGround = false;
    public bool isWater = false;
    public bool gameOver = false;
    public Rigidbody rb;
    private int turns = 1; // 턴 수(리스폰 구역 충돌마다 lifeLeft 감소 방지)
    private int previousTurns = 0; // 이전 턴 수(리스폰 구역 충돌마다 lifeLeft 감소 방지)
    Stage1Manager gm;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = GameObject.Find("Stage1Manager").GetComponent<Stage1Manager>();
    }

    void OnEnable(){
        onGround = false;
        isWater = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    protected virtual void childOnCollisionEnter(Collision collision){}

    void OnCollisionEnter(Collision collision)
    {
        //only called when stickyball 발사 중
        childOnCollisionEnter(collision);

        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }

        if(collision.gameObject.CompareTag("Respawn"))
        {
            isWater = true;
            turns = gm.getTurns();
            Debug.Log("minusLifeLeft at collision: " + gm.getLifeLeft() +", turns: " + turns + ", previousTurns: " + previousTurns);
            if (turns > previousTurns) //몇 턴 진행 후 리스폰 지역 충돌 시 1보다 더 차이나게 되기에
            {
                gm.minusLifeLeft(); //발사 가능 횟수 추가 감소 (벌타)
                Debug.Log("minusLifeLeft at collision: " + gm.getLifeLeft() +", turns: " + turns + ", previousTurns: " + previousTurns);
            }
            previousTurns = turns;
            // gm.lifeLeft--; // 기존
        }
        // if(collision.gameObject.CompareTag("Goal"))
        // {
        //     Debug.Log("Success detected on collisiondetection");
        //     gm.setSuccess(true); //StageManager에서의 처리 위해


        //     // 게임 종료(성공) 카메라 애니메이션 재생 트리거 on
        //     // 해당 크리거 애니메이션 카메라에서 전달받아 애니메이션 재생
        //     // CPC_CameraPath.cs 에서 AnimationEnd(2) 호출
        //     // 게임 종료(성공) 시 햄스터 애니메이션 재생 및 체스트 애니메이션 재생 -> EndGame()에서 구현

        //     // 게임 종료(성공) 카메라 애니메이션 구현 전까지 CPC_CameraPath.cs 대신 임시로 호출
        //     gm.AnimationEnd(2);
        // }
        if(collision.gameObject.CompareTag("Death")){
            gm.setFailure(true); //StageManager에서의 처리 위해
            //원래 시작 위치로 돌아감
            gameOver = true;
        }
    }

    void OnTriggerEnter(Collider other){
        if(other.gameObject.CompareTag("Goal")){
            gm.setSuccess(true); //StageManager에서의 처리 위해
            // 게임 종료(성공) 카메라 애니메이션 재생 트리거 on
            // 해당 크리거 애니메이션 카메라에서 전달받아 애니메이션 재생
            // CPC_CameraPath.cs 에서 AnimationEnd(2) 호출
            // 게임 종료(성공) 시 햄스터 애니메이션 재생 및 체스트 애니메이션 재생 -> Success()에서 구현


            // 게임 종료(성공) 카메라 애니메이션 구현 전까지 CPC_CameraPath.cs 대신 임시로 호출
            // 안하면 EndGame() 호출 안 됨
            // gm.AnimationEnd(2);
        }
    }
}

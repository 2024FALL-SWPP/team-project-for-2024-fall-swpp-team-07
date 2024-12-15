using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public bool onGround = false;
    public bool isWater = false;
    public bool gameOver = false; //필요없는 것 같음

    private int sandCollisionCount = 0;
    private int previousTurnForRespawn = 0; // 턴 전환 시점(대포 이동)이 아닌 턴 첫 모래 지형 충돌 시점 체크 위해 필요
    private int previousTurnForSand = 0; // 턴 전환 시점(대포 이동)이 아닌 턴 첫 모래 지형 충돌 시점 체크 위해 필요
    private bool penalty = false;

    //private bool waitingDelay = false;
    public CannonControl cannonControl; // 필요 없는 것 같음

    // public bool goalIn = false; // 공이 발사되어 골인 됨을 확인하기 위함
    public Rigidbody rb;
    StageManager gm;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gm = FindObjectOfType<StageManager>();
        cannonControl = FindObjectOfType<CannonControl>();
    }

    void OnEnable()
    {
        onGround = false;
        isWater = false;
    }

    protected virtual void childOnCollisionEnter(Collision collision) { }

    void OnCollisionEnter(Collision collision)
    {
        // stickyball 발사에 대한 추가 처리
        childOnCollisionEnter(collision);

        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }

        if (collision.gameObject.CompareTag("Respawn"))
        {
            isWater = true;
            if (gm.GetTurn() > previousTurnForRespawn) // 해당 턴의 첫 번째 respawn 지형 충돌 시 진입
            {
                previousTurnForRespawn = gm.GetTurn();
                gm.SetPenalty();
                Debug.Log(
                    "minusLifeLeft at collision: "
                        + gm.GetLifeLeft()
                        + ", turns: "
                        + gm.GetTurn()
                        + ", previousTurns: "
                        + previousTurnForRespawn
                );
            }
        }

        if (collision.gameObject.CompareTag("Death"))
        {
            gameOver = true;
            gm.SetFailure();
        }

        if (collision.gameObject.CompareTag("Lava"))
        {
            // 충돌 지점 가져오기
            ContactPoint contact = collision.contacts[0]; //현재 발생한 충돌의 첫번째 접촉점
            Vector3 collisionPoint = contact.point;

            // 오브젝트를 충돌 지점으로 이동
            transform.position = collisionPoint;

            // Rigidbody 비활성화
            rb.isKinematic = true;

            // 속도와 각속도 0으로 설정
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            gameOver = true;
            gm.SetFailure();

            // 용암 충돌 시 게임 종료 팝업 지연 시 사용.. 우선 주석처리
            // waitingDelay = true;
            // StartCoroutine(DelayFunction());
            // if (!waitingDelay)
            // {
            //     Debug.Log("DelayFunctionDone");
            //     gm.SetFailure();
            // }
        }

        // 모래 지형 구현 계획
        // 1. 공이 모래 지형에 두번째로 닿은 곳에서 정지하도록(한 번 튕기도록)
        // - sticky ball은 그대로 첫번째로 닿은 곳에 정지하는 기능 유지
        // - 새로운 턴에 대해 sandCollisionCount 초기화 및 같은 턴에서 재진입 시 카운트 증가 구현
        // 2. sticky ball의 경우 한 게임(스테이지 플레이 1회)에 대해 모래 지형에 닿으면 sticky 기능 상실
        // - StickyBallCollision.cs 에서 구현
        // - 다시 플레이 하기, 스테이지 재진입 시 초기화 구현 필요(알아서 될 듯)
        // 3. bowling ball의 경우 첫번째로 닿은 곳에 정지하는 기능 구현

        //StickToCollisionPoint 호출하는 경우 키네마틱 이슈로 발사 시에만 호출되도록 spaceBarCount == 1
        if (collision.gameObject.CompareTag("Sand") && cannonControl.spaceBarCount == 1) // 1 구현
        {
            // 대포 생성하려면 필요(모래도 땅이니까..)
            // 첫 턴에 sand에만 닿을 경우 고려
            onGround = true;
            Debug.Log("Turn: " + gm.GetTurn() + ", PreviousTurn: " + gm.GetPreviousTurn());

            if (gm.GetTurn() > previousTurnForSand) // 해당 턴의 첫 번째 모래 지형 충돌 시 진입
            {
                sandCollisionCount = 1;
                previousTurnForSand = gm.GetTurn();
            }
            else
            {
                sandCollisionCount++;
                if (sandCollisionCount >= 2) // 모래 지형에 두 번째로 닿았을 때 정지
                {
                    StickToCollisionPoint(collision);
                }
            }
        }
    }

    public void StickToCollisionPoint(Collision collision)
    {
        // 충돌 지점 가져오기
        ContactPoint contact = collision.contacts[0]; //현재 발생한 충돌의 첫번째 접촉점
        Vector3 collisionPoint = contact.point;

        // 오브젝트를 충돌 지점으로 이동
        transform.position = collisionPoint;

        // Rigidbody 비활성화
        rb.isKinematic = true;

        // 속도와 각속도 0으로 설정
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    // private IEnumerator DelayFunction() // 용암 충돌 시 게임 종료 팝업 지연 시 사용.. 우선 주석처리
    // {
    //     Debug.Log("DelayFunction");
    //     yield return new WaitForSeconds(3f);
    //     waitingDelay = false;
    // }
}

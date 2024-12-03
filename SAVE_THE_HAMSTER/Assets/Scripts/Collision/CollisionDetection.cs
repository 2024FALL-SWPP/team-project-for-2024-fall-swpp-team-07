using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public bool onGround = false;
    public bool isWater = false;
    public bool gameOver = false; //필요없는 것 같음
    //private bool waitingDelay = false;
    CannonControl cannonControl;

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
            if (gm.GetTurn() > gm.GetPreviousTurn()) //몇 턴 진행 후 리스폰 지역 충돌 시 1보다 더 차이나게 되기에
            {
                gm.DecreaseLifeLeft(); //발사 가능 횟수 추가 감소 (벌타)
                Debug.Log(
                    "minusLifeLeft at collision: "
                        + gm.GetLifeLeft()
                        + ", turns: "
                        + gm.GetTurn()
                        + ", previousTurns: "
                        + gm.GetPreviousTurn()
                );
            }
            gm.UpdatePreviousTurn();
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

            // waitingDelay = true;
            // StartCoroutine(DelayFunction());
            // if (!waitingDelay)
            // {
            //     Debug.Log("DelayFunctionDone");
            //     gm.SetFailure();
            // }
            
        }

        if (collision.gameObject.CompareTag("Sand"))
        {
            Debug.Log("Sand collision");
        }
    }

    // private IEnumerator DelayFunction()
    // {        
    //     Debug.Log("DelayFunction");
    //     yield return new WaitForSeconds(3f);
    //     waitingDelay = false;
    // }
}

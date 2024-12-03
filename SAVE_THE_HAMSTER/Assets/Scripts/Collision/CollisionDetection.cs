using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public bool onGround = false;
    public bool isWater = false;
    public bool gameOver = false;
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
            gm.SetFailure();
            gameOver = true;
        }


        /// 12.03 추가
        if (collision.gameObject.CompareTag("Lava"))
        {
            gm.SetFailure();
            gameOver = true;
        }
        ///
    }
}

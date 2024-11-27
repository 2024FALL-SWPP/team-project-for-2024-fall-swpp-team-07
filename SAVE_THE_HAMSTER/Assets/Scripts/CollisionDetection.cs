using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public bool onGround = false;
    public bool isWater = false;
    public bool gameOver = false;
    private Rigidbody rb;
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true;
        }

        if(collision.gameObject.CompareTag("Respawn"))
        {
            isWater = true;
            gm.lifeLeft--; //발사 가능 횟수 추가 감소 (벌타)
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
}

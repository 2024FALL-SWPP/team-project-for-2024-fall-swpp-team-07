using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaCollision : MonoBehaviour
{
    StageManager gm;

    public ParticleSystem lavaCollisionParticle; // 인스펙터에서 파티클 시스템 할당

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<StageManager>();
    }

    // Update is called once per frame
    void Update() { }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 충돌 지점 가져오기
            ContactPoint contact = collision.contacts[0];
            Vector3 collisionPoint = contact.point;

            // 파티클 위치를 충돌 지점으로 이동
            lavaCollisionParticle.transform.position = collisionPoint;

            // 파티클 재생
            lavaCollisionParticle.Play();
        }

        // cannon collision class 없기에 예외적으로 lava collision에서 처리
        if (collision.gameObject.CompareTag("Cannon"))
        {
            // 충돌 지점 가져오기
            ContactPoint contact = collision.contacts[0];
            Vector3 collisionPoint = contact.point;

            // 파티클 위치를 충돌 지점으로 이동
            lavaCollisionParticle.transform.position = collisionPoint;

            // 파티클 재생
            lavaCollisionParticle.Play();

            gm.SetFailure();
        }
    }
}

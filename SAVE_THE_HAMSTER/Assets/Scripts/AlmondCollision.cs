using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmondCollision : MonoBehaviour
{
    Stage1Manager gm;
    public int almondNumber;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("Stage1Manager").GetComponent<Stage1Manager>();
    }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Rigidbody otherRb = other.gameObject.GetComponent<Rigidbody>();
            // if (otherRb.useGravity) //대포에 붙은 채로 닿으면 인식 안하도록
            // {
            gameObject.SetActive(false);
            gm.setAlmondStatus(almondNumber, true);
            // }
            //gm.GetAlmond(almondNumber); // 기존
        }
    }
}

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
            gameObject.SetActive(false);
            gm.setAlmondStatus(almondNumber, true);
            //gm.GetAlmond(almondNumber); // 기존
        }
    }
}

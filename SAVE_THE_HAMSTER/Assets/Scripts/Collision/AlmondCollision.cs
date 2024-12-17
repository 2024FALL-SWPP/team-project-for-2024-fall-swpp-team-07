using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmondCollision : MonoBehaviour
{
    StageManager gm;
    public int almondNumber;
    CannonControl cannonControl;

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<StageManager>();
        cannonControl = FindObjectOfType<CannonControl>();
    }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        if (cannonControl == null)
        {
            cannonControl = FindObjectOfType<CannonControl>();
        }

        // 턴 중에 아몬드 획득해야 인정됨
        if (other.CompareTag("Player") && cannonControl.spaceBarCount == 1)
        {
            gameObject.SetActive(false);
            gm.GetAlmond(almondNumber);
        }
    }
}

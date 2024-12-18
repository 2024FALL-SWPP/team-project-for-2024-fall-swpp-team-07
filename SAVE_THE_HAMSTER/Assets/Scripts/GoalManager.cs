using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    public GameObject key;
    CannonControl cannonControl;
    StageManager gm;

    // Start is called before the first frame update
    void Start()
    {
        cannonControl = FindObjectOfType<CannonControl>();
        gm = FindObjectOfType<StageManager>();
    }

    // Update is called once per frame
    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        if (cannonControl == null)
        {
            cannonControl = FindObjectOfType<CannonControl>();
        }

        // 턴 중에 골인 지점에 도달하면 성공
        if (other.CompareTag("Player") && cannonControl.spaceBarCount == 1)
        {
            key.SetActive(false);
            gm.SetSuccess();
        }
    }
}

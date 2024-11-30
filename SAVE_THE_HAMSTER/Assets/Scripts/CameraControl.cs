using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Rider.Unity.Editor;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera mainCamera;
    public Camera subCamera;
    public GameObject cannon; //대포 전체
    public GameObject canon; //포신
    private GameObject activeBall;
    public Vector3 offset1;
    public Vector3 offset2;
    public Vector3 offset3;

    CannonControl cannonControl;

    StageManager gm;

    void Start()
    {
        gm = FindObjectOfType<StageManager>();
        cannonControl = cannon.GetComponent<CannonControl>();
        subCamera.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateCamera1(); //mainCamera시점으로 전환
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateCamera2(); //subCamera시점으로 전환
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cannon.activeSelf)
        {
            activeBall = gm.GetActiveBall();
            Rigidbody rb = activeBall.GetComponent<Rigidbody>();
            subCamera.transform.position = cannon.transform.position + offset2;
            subCamera.transform.LookAt(cannon.transform.position);

            //공 발사 전 mainCamera
            if (cannonControl.spaceBarCount == 0)
            {
                Vector3 desiredPosition =
                    cannon.transform.position + cannon.transform.rotation * offset1;

                mainCamera.transform.position = desiredPosition;

                mainCamera.transform.rotation = cannon.transform.rotation;

                mainCamera.transform.LookAt(cannon.transform.position);
            }
            //공 발사 후 mainCamera
            else if (activeBall.transform.position.y >= 0.3f && rb.velocity.magnitude >= 0.1f)
            {
                mainCamera.transform.position = activeBall.transform.position + offset3;
                mainCamera.transform.LookAt(activeBall.transform.position);
            }
        }
    }

    void ActivateCamera1() // 대포 뒤 시점
    {
        mainCamera.gameObject.SetActive(true);
        subCamera.gameObject.SetActive(false);
    }

    void ActivateCamera2() // 대포 위 시점
    {
        mainCamera.gameObject.SetActive(false);
        subCamera.gameObject.SetActive(true);
    }
}

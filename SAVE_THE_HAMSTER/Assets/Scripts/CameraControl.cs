using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
using Cysharp.Threading.Tasks.Triggers;
using JetBrains.Rider.Unity.Editor;
using UnityEngine;

public class CameraControl : MonoBehaviour 
{
    // Start is called before the first frame update
    public Camera mainCamera;
    public Camera subCamera1;
    public Camera subCamera2;
    public GameObject cannon; //대포 전체
    public GameObject canon; //포신
    private GameObject activeBall;
    public Vector3 offset1;
    public Vector3 offset2;
    public Vector3 offset3;
    public Vector3 offset4;

    private bool isYLocked;

    private Vector3 mainLastPosition; //mainCamera의 이전 프레임 위치

    CannonControl cannonControl;

    StageManager gm;

    void Start()
    {
        gm = FindObjectOfType<StageManager>();
        cannonControl = cannon.GetComponent<CannonControl>();
        subCamera2.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        subCamera1.gameObject.SetActive(false);
        mainLastPosition = mainCamera.transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateCamera1(); //대포 바로 뒤 시점으로 전환
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateCamera2(); //대포 뒤 사선 시점으로 전환
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateCamera3(); //대포 위 시점으로 전환
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cannon.activeSelf)
        {
            activeBall = gm.GetActiveBall();
            Rigidbody rb = activeBall.GetComponent<Rigidbody>();
            subCamera1.transform.position = cannon.transform.position + offset2;
            subCamera1.transform.LookAt(cannon.transform.position);
            
            Vector3 desiredPosition2 = canon.transform.position + canon.transform.rotation * offset4;
            subCamera2.transform.position = desiredPosition2;
            subCamera2.transform.rotation = canon.transform.rotation;
            subCamera2.transform.LookAt(canon.transform.position);

            //공 발사 전 mainCamera
            if (cannonControl.spaceBarCount == 0)
            {
                isYLocked = false;
                Vector3 desiredPosition1 =
                    cannon.transform.position + cannon.transform.rotation * offset1;

                mainCamera.transform.position = desiredPosition1;

                mainCamera.transform.rotation = cannon.transform.rotation;

                mainCamera.transform.LookAt(cannon.transform.position);
            }
            //공 발사 후 mainCamera
            else 
            {
                ActivateCamera2();
                
                if(rb.velocity.magnitude >= 1f && !isYLocked)
                {
                    mainCamera.transform.position = activeBall.transform.position + offset3;
                    mainCamera.transform.LookAt(activeBall.transform.position);
                }
                else
                {
                   isYLocked = true;
                   Vector3 newPosition = mainCamera.transform.position;
                   newPosition.x = activeBall.transform.position.x + offset3.x;
                   newPosition.z = activeBall.transform.position.z + offset3.z;
                   mainCamera.transform.position = newPosition;
                   mainCamera.transform.LookAt(activeBall.transform.position);
                }
                
                
                
            }
        }
    }

    void ActivateCamera1() // 대포 바로 뒤 시점
    {
        subCamera2.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        subCamera1.gameObject.SetActive(false);
    }
    void ActivateCamera2() // 대포 뒤 사선 시점
    {
        subCamera2.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        subCamera1.gameObject.SetActive(false);
    }

    void ActivateCamera3() // 대포 위 시점
    {
        subCamera2.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        subCamera1.gameObject.SetActive(true);
    }

    
}

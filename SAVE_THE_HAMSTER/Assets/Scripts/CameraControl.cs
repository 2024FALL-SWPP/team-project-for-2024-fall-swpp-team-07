using System.Collections;
using System.Collections.Generic;
using System.Net;

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
    public Transform firePoint; //탄환 발사 지점
    private GameObject activeBall;
    public Vector3 offset1;
    public Vector3 offset2;
    private Vector3 offset3;
    public Vector3 offset4;
    private float deltay = 3f;

    private bool blockAlpha1;
    private bool blockAlpha2;
    private bool blockAlpha3;
    private bool isYLocked;

    private Vector3 mainLastPosition; //mainCamera의 이전 프레임 위치

    CannonControl cannonControl;

    StageManager gm;

    void Start()
    {
        gm = FindObjectOfType<StageManager>();
        cannonControl = cannon.GetComponent<CannonControl>();
        ActivateCamera1();
        mainLastPosition = mainCamera.transform.position;
        blockAlpha1 = false;
        blockAlpha2 = false;
        blockAlpha3 = false;
        isYLocked = false;
    }

    void Update()
    {
        if (!blockAlpha1 && Input.GetKeyDown(KeyCode.Alpha1))
        {
            ActivateCamera1(); //대포 바로 뒤 시점으로 전환
        }
        if (!blockAlpha2 && Input.GetKeyDown(KeyCode.Alpha2))
        {
            ActivateCamera2(); //대포 뒤 사선 시점으로 전환
        }
        if (!blockAlpha3 && Input.GetKeyDown(KeyCode.Alpha3))
        {
            ActivateCamera3(); //대포 위 시점으로 전환
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (cannon.activeSelf)
        {
            if(cannonControl.spaceBarCount == 0)
            {
                activeBall = gm.GetActiveBall();
                Rigidbody rb = activeBall.GetComponent<Rigidbody>();
                blockAlpha1 = false;
                blockAlpha2 = false;
                blockAlpha3 = false;
                subCamera1.transform.position = cannon.transform.position + offset2;
                subCamera1.transform.LookAt(cannon.transform.position);

                // 대포보다 살짝 위 (시야각 확보)
                Vector3 desiredPosition2 =
                    canon.transform.position + canon.transform.rotation * offset4;
                desiredPosition2.y += deltay;
                subCamera2.transform.position = desiredPosition2;

                // 포신을 바라보는 시점
                Vector3 lookAtPosition = canon.transform.position;
                lookAtPosition.y += deltay;
                subCamera2.transform.LookAt(lookAtPosition);

                //공 발사 전 mainCamera
                isYLocked = false;
                Vector3 desiredPosition1 =
                    cannon.transform.position + cannon.transform.rotation * offset1;
                mainCamera.transform.position = desiredPosition1;
                mainCamera.transform.rotation = cannon.transform.rotation;
                mainCamera.transform.LookAt(cannon.transform.position);
            }
        
            else
            {
                activeBall = gm.GetActiveBall();
                Rigidbody rb = activeBall.GetComponent<Rigidbody>();
                blockAlpha1 = true;
                blockAlpha2 = true;
                blockAlpha3 = true;
                ActivateCamera2();
                offset3 = new Vector3(5 * (canon.transform.position.x - firePoint.position.x), 5 * (firePoint.position.y - canon.transform.position.y) + 5f, 5 * (canon.transform.position.z - firePoint.position.z)) ;
                mainCamera.transform.position = activeBall.transform.position + offset3;
                Vector3 lookAtPosition = activeBall.transform.position;
                mainCamera.transform.LookAt(lookAtPosition);
            }
        }
    }

    public void ActivateCamera1() // 대포 바로 뒤 시점
    {
        subCamera2.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        subCamera1.gameObject.SetActive(false);
    }

    public void ActivateCamera2() // 대포 뒤 사선 시점
    {
        subCamera2.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        subCamera1.gameObject.SetActive(false);
    }

    public void ActivateCamera3() // 대포 위 시점
    {
        subCamera2.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(false);
        subCamera1.gameObject.SetActive(true);
    }
}

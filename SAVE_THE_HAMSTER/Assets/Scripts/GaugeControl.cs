using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject gaugeBar;
    public GameObject cannon;
    public Image gauge;

    private float fillSpeed = 1f; 

    private bool isIncreasing = true; // gauge bar가 증가 상태인지

    private enum Mode{
        LookAt,
        LookAtInverted, 
        CameraForward,
        CameraForwardInverted, 
    }
    void Start()
    {
        gaugeBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(cannon.activeSelf){
            CannonControl cannonControl = GameObject.Find("Cannon").GetComponent<CannonControl>();
            gaugeBar.SetActive(true); // gaugeBar 활성화
            //gaugeBar.transform.position = cannon.transform.position + new Vector3(5f,5f,5f);
            if(cannonControl.spaceBarCount == 0){
                if (isIncreasing)//증가 상태일때
                {
                    gauge.fillAmount += fillSpeed * Time.deltaTime;
                    // 최대치에 도달하면 감소로 전환
                    if (gauge.fillAmount >= 1f)
                    {
                        gauge.fillAmount = 1f; 
                        isIncreasing = false; // 감소로 전환
                    }
                }
                else // 감소 상태일 때
                {
                    gauge.fillAmount -= fillSpeed * Time.deltaTime;
                    // 최소치에 도달하면 증가로 전환
                    if (gauge.fillAmount <= 0f)
                    {
                        gauge.fillAmount = 0f; 
                        isIncreasing = true; // 증가로 전환
                    }
                }
            }
            else if(!cannonControl.spacePressed)
            {
                gauge.fillAmount = 0f; //gauge 값 초기화
            }
               
            
        }

    }
      

    // [SerializeField] private Mode mode;
    // private void LateUpdate() {
    //     if (!gaugeBar.activeSelf) return; // 활성화된 경우에만 처리
    //     switch (mode) { //4가지 mode중 선택 가능
    //         case Mode.LookAt:
    //             if(gaugeBar.activeSelf){
    //             gaugeBar.transform.LookAt(Camera.current.transform);
    //             gaugeBar.transform.Rotate(0f,0f,90f);
    //             }
    //             break;
    //         case Mode.LookAtInverted:
    //             if(gaugeBar.activeSelf){
    //             Vector3 directionFromCamera = gaugeBar.transform.position - Camera.current.transform.position;
    //             gaugeBar.transform.LookAt(gaugeBar.transform.position + directionFromCamera);
    //             gaugeBar.transform.Rotate(0f,0f,90f);
    //             }
    //             break;
    //         case Mode.CameraForward:
    //             //카메라 방향으로 Z축 좌표를 바꿔주기
    //             if(gaugeBar.activeSelf){
    //             gaugeBar.transform.forward = Camera.current.transform.forward;
    //             gaugeBar.transform.Rotate(0f,0f,90f);
    //             }
    //             break;
    //         case Mode.CameraForwardInverted:
    //             //카메라 방향으로 Z축 좌표를 바꿔주고 반전
    //             if(gaugeBar.activeSelf){
    //             gaugeBar.transform.forward = -Camera.current.transform.forward;
    //             gaugeBar.transform.Rotate(0f,0f,90f);
    //             }
    //             break;
    //         default :
                
    //             break;
    //     }
    // }
   
}

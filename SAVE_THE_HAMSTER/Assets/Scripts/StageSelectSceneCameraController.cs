using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectSceneCameraController : MonoBehaviour
{
    private float mouseScrollValue;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        mouseScrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScrollValue > 0 && transform.position.x < 1.2f) // move camera to right when scrolling down
        {
            transform.Translate(Vector3.right * mouseScrollValue);
        }
        else if (mouseScrollValue < 0 && transform.position.x > -1.2f) // move camera to left when scrolling up
        {
            transform.Translate(Vector3.right * mouseScrollValue);
        }
    }
}

//마우스가 클린한 preview의 stage를 파악하고 이를 저장, start버튼 클릭 시 해당 stageScene으로 이동
// public class MouseClickIndicator
// {
//     Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//     RaycastHit hit;
//     private int stageNumber = 0;

//     void Start()
//     {

//     }

//     void Update()   
//     {
//         if (Physics.Raycast(ray, out hit))
//         {
//             switch (hit.transform.gameObject.tag)
//             {
//                 case "Stage1Preview":
//                     stageNumber = 1;
//                     break;
//                 case "Stage2Preview":
//                     stageNumber = 2;
//                     break;
//                 case "Stage3Preview":
//                     stageNumber = 3;
//                     break;
//                 case "Stage4Preview":
//                     stageNumber = 4;
//                     break;
//                 default:
//                     break;
//             }
//         }
//         Debug.Log(stageNumber);
//     }

//     int GetStageNumber()
//     {
//         return stageNumber;
//     }
// }
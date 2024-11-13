using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickIndicator : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;
    private int stageNumber = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // if (Physics.Raycast(ray, out hit))
        // {
        //     switch (hit.transform.gameObject.tag)
        //     {
        //         case "Stage1Preview":
        //             stageNumber = 1;
        //             break;
        //         case "Stage2Preview":
        //             stageNumber = 2;
        //             break;
        //         case "Stage3Preview":
        //             stageNumber = 3;
        //             break;
        //         case "Stage4Preview":
        //             stageNumber = 4;
        //             break;
        //         default:
        //             break;
        //     }
        // }
        // Debug.Log(stageNumber);
    }

    void OnMouseDown()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            switch (hit.transform.gameObject.tag)
            {
                case "Stage1Preview":
                    stageNumber = 1;
                    break;
                case "Stage2Preview":
                    stageNumber = 2;
                    break;
                case "Stage3Preview":
                    stageNumber = 3;
                    break;
                case "Stage4Preview":
                    stageNumber = 4;
                    break;
                default:
                    break;
            }
        }
        Debug.Log(stageNumber);
    }
    
    int GetStageNumber()
    {
        return stageNumber;
    }
}

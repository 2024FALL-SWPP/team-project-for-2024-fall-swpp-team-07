using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // UI 컴포넌트 사용을 위해 추가

public class StageSelector : MonoBehaviour
{
    private int stageNumber = 1;
    private float cameraPositionX;
    public GameObject emphasizePlane;
    //public TMP_Text stageText;
    // 스테이지 설명 추후 작성하기
    // private string [] stageExplanation = {"Stage1: ",
    //                                       "Stage2: ",
    //                                       "Stage3: ",
    //                                       "Stage4: "};

    // Start is called before the first frame update
    void Start()
    {
        cameraPositionX = -3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) && stageNumber > 1)
        {
            stageNumber--;
            cameraPositionX -= 2.0f;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && stageNumber < 4)
        {
            stageNumber++;
            cameraPositionX += 2.0f;
        }
        transform.position = new Vector3(cameraPositionX, transform.position.y, transform.position.z);
        emphasizePlane.transform.position = new Vector3(cameraPositionX, emphasizePlane.transform.position.y, emphasizePlane.transform.position.z);
        //UpdateStageText();
    }

    // void UpdateStageText()
    // {
    //     stageText.text = stageExplanation[stageNumber - 1];
    // }

    public int getStageNumber()
    {
        return stageNumber;
    }
}

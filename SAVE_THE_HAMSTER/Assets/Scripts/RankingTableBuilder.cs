using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RankingTableBuilder : MonoBehaviour
{
    private StageSelector stageSelector;
    private int stageNumber;

    public TMP_Text testText;

    public TMP_Text[] playerText = new TMP_Text[10];
    public TMP_Text[] shotsText = new TMP_Text[10];
    public TMP_Text[] playtimeText = new TMP_Text[10];

    // Start is called before the first frame update
    void Start()
    {
        // find stageSelector to get stage number
        stageSelector = FindObjectOfType<StageSelector>();
        stageNumber = stageSelector.getStageNumber();

        // connect with database

        // get user_id[10] of top ranker matching stage_id == stageNumber
        testText.text = "test";

        // build ranking table
        // for (int i = 0; i < 10; i++)
        // {
        //     playerText[i].text = "" + (i + 1) + "ranker";
        //     shotsText[i].text = "" + 2 * i;
        //     playtimeText[i].text = "" + i + ":" + 30;
        // }
    }

    // Update is called once per frame
    void Update() { }
}

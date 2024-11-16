using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RankingTableBuilder : MonoBehaviour
{
    private SceneTransitionManager sceneTransitionManager;
    private int stageNumber;

    public TMP_Text[] rankingText = new TMP_Text[10];
    public TMP_Text[] playerText = new TMP_Text[10];
    public TMP_Text[] shotsText = new TMP_Text[10];
    public TMP_Text[] playtimeText = new TMP_Text[10];

    // Start is called before the first frame update
    void Start()
    {
        // find sceneTransitionManager to get stage number
        sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
        stageNumber = sceneTransitionManager.getStageNumber();

        // connect with database

        // get user_id[10] of top ranker matching stage_id == stageNumber

        // build ranking table
        for (int i = 0; i < 10; i++)
        {
            rankingText[i].text = "" + (i + 1);
            playerText[i].text = "Player" + i;
            shotsText[i].text = "" + i + "타";
            playtimeText[i].text = "01:3" + i + ".00";
        }
    }

    // Update is called once per frame
    void Update() { }
}

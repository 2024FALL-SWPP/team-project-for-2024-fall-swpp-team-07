using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreStageSceneBackgroundController : MonoBehaviour
{
    private SceneTransitionManager sceneTransitionManager;
    private int stageNumber;
    public GameObject[] background = new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            background[i].SetActive(false);
        }
        sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
        stageNumber = sceneTransitionManager.getStageNumber();
        if (stageNumber != 0)
        {
            background[stageNumber - 1].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

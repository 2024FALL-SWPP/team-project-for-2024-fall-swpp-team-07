using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage1TutorialManager : TutorialManager
{
    public static Stage1TutorialManager tutorialManager = new Stage1TutorialManager();
    // Start is called before the first frame update
    // Stage1Manager의 Start()에서 호출
    public void Start()
    {
        //Stage1TutorialManager tutorialManager = new Stage1TutorialManager();
        //tutorialManager.tutorialNote[0].SetActive(true);
    }

    // Update is called once per frame
    // Stage1Manager의 Update()에서 호출
    public void Update()
    {
        
    }
    
}
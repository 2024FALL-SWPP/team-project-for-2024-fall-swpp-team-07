using System.Collections;
using System.Collections.Generic;
using com.example;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("StartSceneManager Start");
        SoundManager.Instance.PlayStartBGM();
        SoundManager.Instance.InitializeVolume();
    }
}

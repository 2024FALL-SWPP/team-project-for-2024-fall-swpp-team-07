using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoreSceneManager : MonoBehaviour
{
    public GameObject goBackButton;

    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        goBackButton
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                SceneManager.LoadScene("LoadingScene");
            });
    }

    void Update() { }
}

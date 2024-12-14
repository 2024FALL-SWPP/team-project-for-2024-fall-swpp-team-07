using System;
using com.example;
using com.example.Models;
using TMPro;
using UnityEngine;

public class LoadingSceneManager : MonoBehaviour
{
    public TMP_Text almondsText;

    private async void Start()
    {
        SoundManager.Instance.PlayLoadingBGM();
        try
        {
            var client = SupabaseManager.Instance.Supabase();
            if (client != null && client.Auth.CurrentSession != null)
            {
                var userId = client.Auth.CurrentSession.User.Id;

                // UserProfile 테이블에서 almonds 수 가져오기
                var userProfile = await client
                    .From<UserProfile>()
                    .Select("almonds")
                    .Where(x => x.user_id == userId)
                    .Single();

                if (userProfile != null)
                {
                    // almonds 수를 UI에 표시
                    almondsText.text = $"{userProfile.almonds}";
                    Debug.Log($"Successfully loaded almonds count: {userProfile.almonds}");
                }
            }
            else
            {
                Debug.LogWarning("No active session found");
                UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading user data: {e.Message}");
        }
    }

    // Update is called once per frame
    void Update() { }
}

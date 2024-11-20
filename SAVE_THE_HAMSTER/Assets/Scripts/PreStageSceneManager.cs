using System;
using com.example;
using com.example.Models;
using UnityEngine;

public class PreStageSceneManager : MonoBehaviour
{
    private async void Start()
    {
        try
        {
            var client = SupabaseManager.Instance.Supabase();
            if (client != null && client.Auth.CurrentSession != null)
            {
                var userId = client.Auth.CurrentSession.User.Id;
                Debug.Log($"PreStageScene - Current User ID: {userId}");
            }
            else
            {
                Debug.LogWarning("No active session found");
                UnityEngine.SceneManagement.SceneManager.LoadScene("LoginScene");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error in PreStageScene: {e.Message}");
        }
    }

    // Update is called once per frame
    void Update() { }
}

using System;
using Supabase;
using Supabase.Gotrue;
using TMPro;
using UnityEngine;
using Client = Supabase.Client;

namespace com.example
{
    public class SupabaseManager : MonoBehaviour
    {
        // Public Unity references
        public SessionListener SessionListener = null!;
        public SupabaseSettings SupabaseSettings = null!;

        public TMP_Text ErrorText = null!;

        // Internals
        private Client? _client;

        public Client? Supabase() => _client;

        private async void Start()
        {
            try
            {
                Debug.Log($"Attempting to connect with URL: {SupabaseSettings.SupabaseURL}");

                var options = new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true,
                };

                // Supabase 클라이언트 초기화
                _client = new Client(
                    SupabaseSettings.SupabaseURL,
                    SupabaseSettings.SupabaseAnonKey,
                    options
                );

                // 네트워크 체크를 건너뛰고 직접 초기화
                _client.Auth.Online = true;
                await _client.InitializeAsync();

                // 연결 상태 확인
                var settings = await _client.Auth.Settings();
                if (settings != null)
                {
                    Debug.Log("Supabase 연결 성공!");
                }
            }
            catch (Exception e)
            {
                ErrorText.text = $"초기화 에러: {e.Message}";
                Debug.LogError($"초기화 에러: {e.Message}");
            }
        }

        private void OnApplicationQuit()
        {
            if (_client != null)
            {
                _client.Auth.Shutdown();
                _client = null;
            }
        }
    }
}

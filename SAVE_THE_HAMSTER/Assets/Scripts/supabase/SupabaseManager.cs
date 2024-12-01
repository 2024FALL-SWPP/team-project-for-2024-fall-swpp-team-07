using System;
using Supabase;
using Supabase.Gotrue;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Client = Supabase.Client;

namespace com.example
{
    public class SupabaseManager : MonoBehaviour
    {
        // use Singleton pattern
        private static SupabaseManager instance;
        public static SupabaseManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SupabaseManager>();
                    if (instance == null)
                    {
                        instance = new GameObject(
                            "SupabaseManager"
                        ).AddComponent<SupabaseManager>();
                    }
                }
                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "LoginScene")
            {
                ErrorText = GameObject.Find("Error Label").GetComponent<TMP_Text>();
            }
        }

        // Public Unity references
        public SessionListener SessionListener = null!;
        public SupabaseSettings SupabaseSettings = null!;

        public TMP_Text ErrorText = null!;

        // Internals
        private Client? _client;

        public Client? Supabase() => _client;

        private static int _guestCounter = (int)(DateTime.Now.Ticks % 1000000);

        public static int GetNextGuestNumber()
        {
            return ++_guestCounter;
        }

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

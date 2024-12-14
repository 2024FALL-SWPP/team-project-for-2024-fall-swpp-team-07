using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace com.example
{
    public class SettingManager : MonoBehaviour
    {
        // use Singleton pattern
        private static SettingManager instance;
        public static SettingManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SettingManager>();
                    if (instance == null)
                    {
                        instance = new GameObject("SettingManager").AddComponent<SettingManager>();
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

        // variables
        public Canvas SettingCanvas;
        public GameObject BrightnessPanel;
        private Image brightnessImage;
        private int opacity = 0;
        public bool disableTutorial = false;

        // buttons
        public Button brightnessUpButton;
        public Button brightnessDownButton;
        public Button volumeUpButton;
        public Button volumeDownButton;
        public Button tutorialEnabledButton;
        public Button tutorialDisabledButton;
        public Button closeButton;
        public Button logoutButton;

        private void Start()
        {
            // add listeners to buttons
            brightnessUpButton.onClick.AddListener(OnBrightnessUp);
            brightnessDownButton.onClick.AddListener(OnBrightnessDown);
            volumeUpButton.onClick.AddListener(OnVolumeUp);
            volumeDownButton.onClick.AddListener(OnVolumeDown);
            tutorialEnabledButton.onClick.AddListener(OnDisableTutorial);
            tutorialDisabledButton.onClick.AddListener(OnEnableTutorial);
            closeButton.onClick.AddListener(OnClose);
            logoutButton.onClick.AddListener(OnLogout);
            brightnessImage = BrightnessPanel.GetComponent<Image>();
        }

        // 밝기 조절
        private void AdjustBrightness(bool isIncrease)
        {
            if (brightnessImage == null)
            {
                brightnessImage = BrightnessPanel.GetComponent<Image>();
            }

            if (isIncrease && opacity > 0)
            {
                opacity -= 20;
                Color color = brightnessImage.color;
                color.a = opacity / 255f;
                brightnessImage.color = color;
            }
            else if (!isIncrease && opacity < 180)
            {
                opacity += 20;
                Color color = brightnessImage.color;
                color.a = opacity / 255f;
                brightnessImage.color = color;
            }
        }

        public void OnBrightnessUp()
        {
            AdjustBrightness(true);
        }

        public void OnBrightnessDown()
        {
            AdjustBrightness(false);
        }

        private void AdjustVolume(bool isIncrease)
        {
            if (isIncrease)
            {
                SoundManager.Instance.IncreaseVolume();
            }
            else
            {
                SoundManager.Instance.DecreaseVolume();
            }
        }

        public void OnVolumeUp()
        {
            AdjustVolume(true);
        }

        public void OnVolumeDown()
        {
            AdjustVolume(false);
        }

        public void OnDisableTutorial()
        {
            disableTutorial = true;
            tutorialEnabledButton.gameObject.SetActive(false);
            tutorialDisabledButton.gameObject.SetActive(true);
        }

        public void OnEnableTutorial()
        {
            disableTutorial = false;
            tutorialEnabledButton.gameObject.SetActive(true);
            tutorialDisabledButton.gameObject.SetActive(false);
        }

        public void OnClose()
        {
            SettingCanvas.gameObject.SetActive(false);
        }

        public void OnOpen()
        {
            SettingCanvas.gameObject.SetActive(true);
        }

        public async void OnLogout()
        {
            var client = SupabaseManager.Instance.Supabase();
            if (client != null)
            {
                await client.Auth.SignOut();
                // 설정 창 닫기
                SettingCanvas.gameObject.SetActive(false);
                // 튜토리얼 활성화 상태로 설정
                disableTutorial = false;
                tutorialEnabledButton.gameObject.SetActive(true);
                tutorialDisabledButton.gameObject.SetActive(false);
                // 로그인 씬으로 이동
                SceneManager.LoadScene("LoginScene");
            }
        }
    }
}

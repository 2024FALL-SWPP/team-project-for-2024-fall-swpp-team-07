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
        public Button disableTutorialButton;
        public Button enableTutorialButton;
        public Button closeButton;
        public Button logoutButton;

        private void Start()
        {
            // add listeners to buttons
            brightnessUpButton.onClick.AddListener(OnBrightnessUp);
            brightnessDownButton.onClick.AddListener(OnBrightnessDown);
            volumeUpButton.onClick.AddListener(OnVolumeUp);
            volumeDownButton.onClick.AddListener(OnVolumeDown);
            disableTutorialButton.onClick.AddListener(OnDisableTutorial);
            enableTutorialButton.onClick.AddListener(OnEnableTutorial);
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
            // TODO: 음량 조절 로직 구현
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
            disableTutorialButton.image.color = new Color(1f, 1f, 1f, 1f);
            enableTutorialButton.image.color = new Color(1f, 1f, 1f, 0.5f);
        }

        public void OnEnableTutorial()
        {
            disableTutorial = false;
            disableTutorialButton.image.color = new Color(1f, 1f, 1f, 0.5f);
            enableTutorialButton.image.color = new Color(1f, 1f, 1f, 1f);
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
                SettingCanvas.gameObject.SetActive(false);
                SceneManager.LoadScene("LoginScene");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.example;
using com.example.Models;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreSceneManager : MonoBehaviour
{
    public GameObject goBackButton;
    public TMP_Text almondsText;
    public TMP_Text purchaseFailText;

    [Header("Store Items")]
    public Button[] itemButtons = new Button[6];
    public GameObject[] almondImages = new GameObject[6];
    public TMP_Text[] buttonTexts = new TMP_Text[6];

    private string[] columnNames = new string[]
    {
        "hamster_skin_1", // 검은색 햄스터
        "hamster_skin_2", // 흰색 햄스터
        "ball_skin_1", // 빨간색 볼
        "ball_skin_2", // 은색 볼
        "ball_tail_effect_1", // 꼬리 이펙트 1
        "ball_tail_effect_2", // 꼬리 이펙트 2
    };

    private int[] itemPrices = new int[] { 3, 4, 3, 4, 3, 4 };

    private int currentAlmonds;

    private async void Start()
    {
        SoundManager.Instance.PlayStoreBGM();
        try
        {
            var client = SupabaseManager.Instance.Supabase();
            if (client != null && client.Auth.CurrentSession != null)
            {
                var userId = client.Auth.CurrentSession.User.Id;

                // UserProfile에서 almond 수 가져오기
                var userProfile = await client
                    .From<UserProfile>()
                    .Select("almonds")
                    .Where(x => x.user_id == userId)
                    .Single();

                if (userProfile != null)
                {
                    currentAlmonds = userProfile.almonds;
                    almondsText.text = $"{currentAlmonds}";
                }

                // UserStore에서 구매 정보 가져오기
                var userStore = await client
                    .From<UserStore>()
                    .Select("*")
                    .Where(x => x.user_id == userId)
                    .Single();

                if (userStore != null)
                {
                    // 각 아이템의 상태 업데이트
                    for (int i = 0; i < itemButtons.Length; i++)
                    {
                        bool isPurchased = GetPurchaseStatus(userStore, columnNames[i]);
                        UpdateButtonState(i, isPurchased);
                    }
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
            Debug.LogError($"Error loading store data: {e.Message}");
        }

        // 뒤로가기 버튼 이벤트
        goBackButton
            .GetComponent<Button>()
            .onClick.AddListener(() =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScene");
            });
    }

    private void UpdateButtonState(int index, bool isPurchased)
    {
        if (isPurchased)
        {
            buttonTexts[index].text = "보유중";
            almondImages[index].SetActive(false);
            itemButtons[index].onClick.RemoveAllListeners();
        }
        else
        {
            itemButtons[index].onClick.AddListener(() => PurchaseItem(index));
        }
    }

    private bool GetPurchaseStatus(UserStore userStore, string columnName)
    {
        switch (columnName)
        {
            case "hamster_skin_1":
                return userStore.hamster_skin_1;
            case "hamster_skin_2":
                return userStore.hamster_skin_2;
            case "ball_skin_1":
                return userStore.ball_skin_1;
            case "ball_skin_2":
                return userStore.ball_skin_2;
            case "ball_tail_effect_1":
                return userStore.ball_tail_effect_1;
            case "ball_tail_effect_2":
                return userStore.ball_tail_effect_2;
            default:
                return false;
        }
    }

    private async void PurchaseItem(int index)
    {
        try
        {
            var client = SupabaseManager.Instance.Supabase();
            if (client == null || client.Auth.CurrentSession == null)
                return;

            var userId = client.Auth.CurrentSession.User.Id;
            string columnName = columnNames[index];
            int itemPrice = itemPrices[index];

            // 로컬에서 구매 가능 여부 확인
            if (currentAlmonds < itemPrice)
            {
                purchaseFailText.gameObject.SetActive(true);
                await Task.Delay(2000);
                purchaseFailText.gameObject.SetActive(false);
                return;
            }

            // 로컬 아몬드 수 차감
            currentAlmonds -= itemPrice;
            almondsText.text = $"{currentAlmonds}";

            // DB 업데이트
            await client
                .From<UserProfile>()
                .Where(x => x.user_id == userId)
                .Set(x => x.almonds, currentAlmonds)
                .Update();

            // Store 테이블 업데이트
            var userStore = await client.From<UserStore>().Where(x => x.user_id == userId).Single();

            // 리플렉션을 사용하여 동적으로 속성 설정
            var property = typeof(UserStore).GetProperty(columnName);
            property?.SetValue(userStore, true);
            await userStore.Update<UserStore>();

            // 버튼 상태 업데이트
            UpdateButtonState(index, true);
        }
        catch (Exception e)
        {
            Debug.LogError($"Purchase failed: {e.Message}");
        }
    }
}

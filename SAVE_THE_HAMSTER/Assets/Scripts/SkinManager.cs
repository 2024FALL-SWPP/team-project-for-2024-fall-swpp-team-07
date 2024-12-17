using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using com.example.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace com.example
{
    public class SkinManager : MonoBehaviour
    {
        private static SkinManager instance;
        public static SkinManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SkinManager>();
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

            // 씬 변경 시 이벤트 리스너 등록
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private int currentHamsterSkin = 0; // 0: default, 1: skin1, 2: skin2
        private int currentBallSkin = 0; // 0: default, 1: skin1, 2: skin2
        private int currentBallTailEffect = 0; // 0: default, 1: effect1, 2: effect2

        public Material[] hamsterSkinMaterials = new Material[2];
        public Material[] ballSkinMaterials = new Material[2];
        public Material[] stickyBallSkinMaterials = new Material[2];

        private const string HAMSTER_SKIN_LAYER = "HamsterSkin";
        private const string BALL_SKIN_LAYER = "BallSkin";
        private const string STICKY_BALL_SKIN_LAYER = "StickyBallSkin";
        private const string HAMSTER_BALL_SKIN_LAYER = "HamsterBallSkin";

        private async Task LoadSkinInfo()
        {
            // db에서 스킨 정보 로드
            try
            {
                var client = SupabaseManager.Instance.Supabase();
                if (client != null && client.Auth.CurrentSession != null)
                {
                    var userId = client.Auth.CurrentSession.User.Id;

                    // UserProfile에서 almond 수 가져오기
                    var userProfile = await client
                        .From<UserProfile>()
                        .Select(x => new object[] { x.hamster_skin, x.ball_skin, x.tail_skin })
                        .Where(x => x.user_id == userId)
                        .Single();

                    if (userProfile != null)
                    {
                        currentHamsterSkin = userProfile.hamster_skin;
                        currentBallSkin = userProfile.ball_skin;
                        currentBallTailEffect = userProfile.tail_skin;
                    }
                }
                else
                {
                    Debug.LogWarning("No active session found");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error loading store data: {e.Message}");
            }
        }

        private async void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (
                scene.name == "LoadingScene"
                || scene.name == "Stage1Scene"
                || scene.name == "Stage2Scene"
                || scene.name == "Stage3Scene"
            )
            {
                await LoadSkinInfo();
                ApplyAllSkins();
            }
        }

        public void ApplyAllSkins()
        {
            // 햄스터 스킨 적용
            if (currentHamsterSkin > 0)
            {
                GameObject[] hamsterObjects = FindObjectsOfLayer(
                    LayerMask.NameToLayer(HAMSTER_SKIN_LAYER)
                );
                foreach (GameObject obj in hamsterObjects)
                {
                    Renderer renderer = obj.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        renderer.material = hamsterSkinMaterials[currentHamsterSkin - 1];
                    }
                }
            }

            // 볼 스킨 적용
            if (currentBallSkin > 0)
            {
                // 일반 볼 스킨 적용
                GameObject[] ballObjects = FindObjectsOfLayer(
                    LayerMask.NameToLayer(BALL_SKIN_LAYER)
                );
                foreach (GameObject obj in ballObjects)
                {
                    MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.material = ballSkinMaterials[currentBallSkin - 1];
                    }
                }

                // 스티키 볼 스킨 적용
                GameObject[] stickyBallObjects = FindObjectsOfLayer(
                    LayerMask.NameToLayer(STICKY_BALL_SKIN_LAYER)
                );
                foreach (GameObject obj in stickyBallObjects)
                {
                    MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        meshRenderer.material = stickyBallSkinMaterials[currentBallSkin - 1];
                    }
                }

                // 햄스터 볼 스킨 적용
                GameObject[] hamsterBallObjects = FindObjectsOfLayer(
                    LayerMask.NameToLayer(HAMSTER_BALL_SKIN_LAYER)
                );
                foreach (GameObject obj in hamsterBallObjects)
                {
                    MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
                    if (meshRenderer != null)
                    {
                        Material[] materials = meshRenderer.materials;
                        materials[1] = ballSkinMaterials[currentBallSkin - 1];
                        meshRenderer.materials = materials;
                    }
                }
            }

            // 볼 꼬리 이펙트 적용
            if (currentBallTailEffect == 1)
            {
                GameObject[] trailFireObjects = Resources.FindObjectsOfTypeAll<GameObject>();
                GameObject trailFire = System.Array.Find(
                    trailFireObjects,
                    obj => obj.CompareTag("TrailFire")
                );
                if (trailFire != null)
                {
                    trailFire.SetActive(true);
                }
            }
            else if (currentBallTailEffect == 2)
            {
                GameObject[] trailWaterObjects = Resources.FindObjectsOfTypeAll<GameObject>();
                GameObject trailWater = System.Array.Find(
                    trailWaterObjects,
                    obj => obj.CompareTag("TrailWater")
                );
                if (trailWater != null)
                {
                    trailWater.SetActive(true);
                }
            }
        }

        private GameObject[] FindObjectsOfLayer(int layer)
        {
            // https://stackoverflow.com/questions/44456133/find-inactive-gameobject-by-name-tag-or-layer
            // inactive 상태의 GameObject도 찾기 위해 Resources.FindObjectsOfTypeAll 사용
            GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
            List<GameObject> objectsInLayer = new List<GameObject>();
            foreach (GameObject obj in allObjects)
            {
                if (obj.layer == layer)
                {
                    objectsInLayer.Add(obj);
                }
            }
            return objectsInLayer.ToArray();
        }

        public void ResetSkins()
        {
            // 로그아웃 시 호출됨
            currentHamsterSkin = 0;
            currentBallSkin = 0;
            currentBallTailEffect = 0;
        }

        public async void SetHamsterSkin(int skinIndex)
        {
            // StoreSceneManager에서 호출됨
            currentHamsterSkin = skinIndex;
            try
            {
                var client = SupabaseManager.Instance.Supabase();
                if (client != null && client.Auth.CurrentSession != null)
                {
                    var userId = client.Auth.CurrentSession.User.Id;

                    // UserProfile에 스킨 정보 저장
                    await client
                        .From<UserProfile>()
                        .Where(x => x.user_id == userId)
                        .Set(x => x.hamster_skin, skinIndex)
                        .Update();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error saving hamster skin data: {e.Message}");
            }
        }

        public int GetHamsterSkin()
        {
            return currentHamsterSkin;
        }

        public async void SetBallSkin(int skinIndex)
        {
            // StoreSceneManager에서 호출됨
            currentBallSkin = skinIndex;
            try
            {
                var client = SupabaseManager.Instance.Supabase();
                if (client != null && client.Auth.CurrentSession != null)
                {
                    var userId = client.Auth.CurrentSession.User.Id;

                    // UserProfile에 스킨 정보 저장
                    await client
                        .From<UserProfile>()
                        .Where(x => x.user_id == userId)
                        .Set(x => x.ball_skin, skinIndex)
                        .Update();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error saving ball skin data: {e.Message}");
            }
        }

        public int GetBallSkin()
        {
            return currentBallSkin;
        }

        public async void SetBallTailEffect(int effectIndex)
        {
            // StoreSceneManager에서 호출됨
            currentBallTailEffect = effectIndex;
            try
            {
                var client = SupabaseManager.Instance.Supabase();
                if (client != null && client.Auth.CurrentSession != null)
                {
                    var userId = client.Auth.CurrentSession.User.Id;

                    // UserProfile에 스킨 정보 저장
                    await client
                        .From<UserProfile>()
                        .Where(x => x.user_id == userId)
                        .Set(x => x.tail_skin, effectIndex)
                        .Update();
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error saving ball tail effect data: {e.Message}");
            }
        }

        public int GetBallTailEffect()
        {
            return currentBallTailEffect;
        }
    }
}

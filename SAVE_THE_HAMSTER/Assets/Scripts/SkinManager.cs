using System.Collections;
using System.Collections.Generic;
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

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (
                scene.name == "LoadingScene"
                || scene.name == "Stage1Scene"
                || scene.name == "Stage2Scene"
                || scene.name == "Stage3Scene"
            )
            {
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

        public void SetHamsterSkin(int skinIndex)
        {
            // StoreSceneManager에서 호출됨
            currentHamsterSkin = skinIndex;
        }

        public int GetHamsterSkin()
        {
            return currentHamsterSkin;
        }

        public void SetBallSkin(int skinIndex)
        {
            // StoreSceneManager에서 호출됨
            currentBallSkin = skinIndex;
        }

        public int GetBallSkin()
        {
            return currentBallSkin;
        }

        public void SetBallTailEffect(int effectIndex)
        {
            // StoreSceneManager에서 호출됨
            currentBallTailEffect = effectIndex;
        }
    }
}

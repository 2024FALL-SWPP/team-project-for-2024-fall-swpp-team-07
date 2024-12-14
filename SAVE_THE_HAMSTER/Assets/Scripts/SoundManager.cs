using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.example
{
    public class SoundManager : MonoBehaviour
    {
        // use Singleton pattern
        private static SoundManager instance;
        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SoundManager>();
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

        public AudioSource bgmSource;
        public AudioSource sfxSource;

        public AudioClip startBGM;
        public AudioClip loadingBGM;
        public AudioClip storeBGM;
        public AudioClip stage1BGM;
        public AudioClip stage2BGM;
        public AudioClip stage3BGM;
        public AudioClip successFBX;
        public AudioClip failFBX;
        public AudioClip acquireSFX;

        public void PlayBGM(AudioClip bgm, bool loop = true)
        {
            bgmSource.clip = bgm;
            bgmSource.loop = loop;
            bgmSource.Play();
        }

        public void PlaySFX(AudioClip sfx)
        {
            sfxSource.PlayOneShot(sfx);
        }

        public void PlayStartBGM()
        {
            PlayBGM(startBGM);
        }

        public void PlayLoadingBGM()
        {
            PlayBGM(loadingBGM);
        }

        public void PlayStoreBGM()
        {
            PlayBGM(storeBGM);
        }

        public void PlayIngameBGM(int stage)
        {
            switch (stage)
            {
                case 1:
                    PlayBGM(stage1BGM);
                    break;
                case 2:
                    PlayBGM(stage2BGM);
                    break;
                case 3:
                    PlayBGM(stage3BGM);
                    break;
            }
        }

        public void PlaySuccessFBX()
        {
            // stop current bgm
            bgmSource.Stop();
            PlayBGM(successFBX);
        }

        public void PlayFailFBX()
        {
            // stop current bgm
            bgmSource.Stop();
            PlayBGM(failFBX);
        }

        public void PlayAcquireSFX()
        {
            PlaySFX(acquireSFX);
        }

        public void InitializeVolume()
        {
            bgmSource.volume = 1.0f;
            sfxSource.volume = 1.0f;
        }

        public void IncreaseVolume()
        {
            float amount = 0.1f;
            bgmSource.volume = Mathf.Clamp(bgmSource.volume + amount, 0, 1);
            sfxSource.volume = Mathf.Clamp(sfxSource.volume + amount, 0, 1);
        }

        public void DecreaseVolume()
        {
            float amount = 0.1f;
            bgmSource.volume = Mathf.Clamp(bgmSource.volume - amount, 0, 1);
            sfxSource.volume = Mathf.Clamp(sfxSource.volume - amount, 0, 1);
        }
    }
}

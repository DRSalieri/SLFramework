using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLFramework.Singleton;

namespace SLFramework.Manager
{
    /// <summary>
    /// ��Ƶ������������
    /// </summary>
    public class AudioManager : SingletonMono<AudioManager>
    {
        #region Components

        private AudioSource SFXSource;

        private AudioSource BGMSource;

        #endregion

        #region Setup

        protected override void OnAwakeInit()
        {
            base.OnAwakeInit();

            // hierarchy�����ò㼶
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            // ������
            SFXSource = gameObject.AddComponent<AudioSource>();
            BGMSource = gameObject.AddComponent<AudioSource>();
        }

        #endregion

        #region SFX Function

        public void PlaySFX(string name)
        {
            ResourceManager.LoadAsync<AudioClip>(name, clip =>
            {
                if (!clip) return;
                SFXSource.PlayOneShot(clip);
            });
        }

        #endregion

        #region BGM Function

        public void PlayBGM(string name,bool isLoop = true)
        {
            ResourceManager.LoadAsync<AudioClip>(name, clip =>
            {
                if (!clip) return;
                BGMSource.clip = clip;
                BGMSource.loop = isLoop;
                BGMSource.Play();
            });
        }

        public void PauseBGM()
        {
            BGMSource.Pause();
        }

        public void UnPauseBGM()
        {
            BGMSource.UnPause();
        }

        public void StopBGM()
        {
            BGMSource.Stop();
        }

        #endregion

    }
}
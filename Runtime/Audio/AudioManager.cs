using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLFramework.Manager
{
    /// <summary>
    /// 音频管理器，单例
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        #region Setup

        private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                // Manager通用初始化
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        #endregion
    }
}
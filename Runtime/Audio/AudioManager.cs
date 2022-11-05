using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLFramework.Manager
{
    /// <summary>
    /// ��Ƶ������������
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
                // Managerͨ�ó�ʼ��
                transform.parent = null;
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        #endregion
    }
}
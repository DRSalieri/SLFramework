using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLFramework.Singleton;

namespace SLFramework.Manager
{
    /// <summary>
    /// ����������Mono����
    /// ִ�����ȼ�-9
    /// </summary>
    public class GameManager : SingletonMono<GameManager>
    {
        #region Setup

        protected override void OnAwakeInit()
        {
            base.OnAwakeInit();

            // hierarchy�����ò㼶
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }

        #endregion

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLFramework.Singleton;

namespace SLFramework.Manager
{
    /// <summary>
    /// 主管理器，Mono单例
    /// 执行优先级-9
    /// </summary>
    public class GameManager : SingletonMono<GameManager>
    {
        #region Setup

        protected override void OnAwakeInit()
        {
            base.OnAwakeInit();

            // hierarchy中设置层级
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }

        #endregion

    }
}
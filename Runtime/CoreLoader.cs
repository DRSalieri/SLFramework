using SLFramework.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace SLFramework.Core
{
    /// <summary>
    /// 加载Core，使用前必须将Core场景添加到Build Setting中去
    /// 优先级-10
    /// </summary>
    [DefaultExecutionOrder(-10)]
    public class CoreLoader : MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                // 如果不存在GameManager，则表示未加载Core，读取Core场景来加载
                if (!GameManager.Instance)
                    SceneManager.LoadScene("Core", LoadSceneMode.Additive);
                Destroy(gameObject);
            }
            catch(Exception e)
            {
                Debug.LogError("You should add core scene to build settings!");
                throw;
            }
        }
    }
}
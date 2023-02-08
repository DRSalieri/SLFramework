using System;
using System.Collections;
using UnityEngine;
using SLFramework.Singleton;

namespace SLFramework.Manager
{
    /// <summary>
    /// 单例类型的一个管理器
    /// 1.方便调用Update的函数
    /// 2.方便启动协程
    /// </summary>
    public class MonoManager : Singleton<MonoManager>
    {
        private readonly MonoController controller;

        public MonoManager()
        {
            GameObject obj = new GameObject("MonoController");

            controller = obj.AddComponent<MonoController>();

            obj.hideFlags = HideFlags.HideAndDontSave;
            
        }

        public void AddEventListener(Action action) => controller.AddEventListener(action);

        public void RemoveEventListener(Action action) => controller.RemoveEventListener(action);

        public Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return controller.StartCoroutine(coroutine);
        }
    }
}

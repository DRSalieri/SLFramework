using UnityEngine;

namespace SLFramework.Singleton
{
    /// <summary>
    /// 单例Mono基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SingletonMono<T> : MonoBehaviour where T : SingletonMono<T>
    {
        private static T instance;

        public static T Instance => instance;

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = (T)this;
                OnAwakeInit();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Awake中初次初始化时会调用的函数
        /// </summary>
        protected virtual void OnAwakeInit()
        {

        }
    }
}

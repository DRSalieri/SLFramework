using UnityEngine;

namespace SLFramework.Singleton
{
    /// <summary>
    /// ����Mono����
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
        /// Awake�г��γ�ʼ��ʱ����õĺ���
        /// </summary>
        protected virtual void OnAwakeInit()
        {

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using SLFramework.Async;

namespace SLFramework
{
    /// <summary>
    /// ��Դ��������������
    /// </summary>
    public static class ResourceManager
    {
        /// <summary>
        /// ��������ΪT����Դ��nameΪResource�����·��
        /// </summary>
        /// <typeparam name="T">��Դ����</typeparam>
        /// <param name="name">Resource�����·��</param>
        /// <returns>���ؽ��</returns>
        public static T Load<T>(string name) where T : Object
        {
            T resource = Resources.Load<T>(name);

            if(resource is GameObject)
            {
                return Object.Instantiate(resource);
            }

            return resource;
        }

        /// <summary>
        /// �첽���أ���ȴ�������ɡ����ûص������󷵻�
        /// </summary>
        /// <typeparam name="T">��Դ����</typeparam>
        /// <param name="name">Resource�����·��</param>
        /// <param name="callback">������ɺ���õĻص�����</param>
        public static async void LoadAsync<T>(string name, Action<T> callback) where T : Object
        {
            // �ȴ��첽�������
            await LoadCompleted(name, callback);
        }

        private static IEnumerator LoadCompleted<T>(string name, Action<T> callback) where T : Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(name);
            yield return request;
            if(request.asset is GameObject)
            {
                T obj = Object.Instantiate(request.asset) as T;
                if(obj == null)
                {
                    Debug.LogWarning("δ��ȡ����Դ��" + name);
                    yield return null;
                }
                callback(obj);
            }
            else
            {
                T obj = request.asset as T;
                if(obj == null)
                {
                    Debug.LogWarning("δ��ȡ����Դ��" + name);
                    yield return null;
                }
                callback(obj);
            }
        }
        
    }
}
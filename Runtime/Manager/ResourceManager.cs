using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
using SLFramework.Async;

namespace SLFramework
{
    /// <summary>
    /// 资源管理器，方法类
    /// </summary>
    public static class ResourceManager
    {
        /// <summary>
        /// 加载类型为T的资源，name为Resource下相对路径
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="name">Resource下相对路径</param>
        /// <returns>加载结果</returns>
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
        /// 异步加载，会等待加载完成、调用回调函数后返回
        /// </summary>
        /// <typeparam name="T">资源类型</typeparam>
        /// <param name="name">Resource下相对路径</param>
        /// <param name="callback">加载完成后调用的回调函数</param>
        public static async void LoadAsync<T>(string name, Action<T> callback) where T : Object
        {
            // 等待异步加载完成
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
                    Debug.LogWarning("未获取到资源：" + name);
                    yield return null;
                }
                callback(obj);
            }
            else
            {
                T obj = request.asset as T;
                if(obj == null)
                {
                    Debug.LogWarning("未获取到资源：" + name);
                    yield return null;
                }
                callback(obj);
            }
        }
        
    }
}
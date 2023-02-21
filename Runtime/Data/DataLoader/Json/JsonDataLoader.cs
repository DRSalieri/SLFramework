using SLFramework.DataLoader.Excel;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using System.IO;

namespace SLFramework.DataLoader.Json
{
    public static class JsonDataLoader
    {
        /// <summary>
        /// 读取Json文件（Json反序列化类需要提前定义）
        /// 返回一个该对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadJson<T>(string filePath) where T : new()
        {
            if(File.Exists(filePath))
            {
                Debug.Log("(Json Data Loader) 读取json文件成功： " + filePath);
                string jsonStr = File.ReadAllText(filePath);
                T data = JsonConvert.DeserializeObject<T>(jsonStr);
                return data;
            }
            else
            {
                Debug.LogWarning("(Json Data Loader) 读取json文件失败：未找到文件 " + filePath);
                return new T();
            }
        }
    }
}

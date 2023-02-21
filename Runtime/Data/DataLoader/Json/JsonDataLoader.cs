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
        /// ��ȡJson�ļ���Json�����л�����Ҫ��ǰ���壩
        /// ����һ���ö���
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static T LoadJson<T>(string filePath) where T : new()
        {
            if(File.Exists(filePath))
            {
                Debug.Log("(Json Data Loader) ��ȡjson�ļ��ɹ��� " + filePath);
                string jsonStr = File.ReadAllText(filePath);
                T data = JsonConvert.DeserializeObject<T>(jsonStr);
                return data;
            }
            else
            {
                Debug.LogWarning("(Json Data Loader) ��ȡjson�ļ�ʧ�ܣ�δ�ҵ��ļ� " + filePath);
                return new T();
            }
        }
    }
}

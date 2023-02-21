using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLFramework.Data
{
    /// <summary>
    /// 数据的通用接口，需要实现
    /// InitData() - 数据初始化
    /// SaveData() - 数据的保存
    /// LoadData() - 数据的加载
    /// </summary>
    public interface IData
    {
        void InitData();
        void SaveData();
        void LoadData();
    }

    /// <summary>
    /// 数据的基类，继承实现即可
    /// </summary>
    public class BaseData : ScriptableObject, IData
    {
        public virtual void InitData()
        {
        }

        public virtual void SaveData()
        {
        }

        public virtual void LoadData()
        {
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLFramework.Data
{
    /// <summary>
    /// ���ݵ�ͨ�ýӿڣ���Ҫʵ��
    /// InitData() - ���ݳ�ʼ��
    /// SaveData() - ���ݵı���
    /// LoadData() - ���ݵļ���
    /// </summary>
    public interface IData
    {
        void InitData();
        void SaveData();
        void LoadData();
    }

    /// <summary>
    /// ���ݵĻ��࣬�̳�ʵ�ּ���
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
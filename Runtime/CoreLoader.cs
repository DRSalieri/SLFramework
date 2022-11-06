using SLFramework.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

namespace SLFramework.Core
{
    /// <summary>
    /// ����Core��ʹ��ǰ���뽫Core������ӵ�Build Setting��ȥ
    /// ���ȼ�-10
    /// </summary>
    [DefaultExecutionOrder(-10)]
    public class CoreLoader : MonoBehaviour
    {
        private void Awake()
        {
            try
            {
                // ���������GameManager�����ʾδ����Core����ȡCore����������
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
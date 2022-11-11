using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLFramework.Singleton;
using SLFramework.UI;

namespace SLFramework.Manager
{

    public class UIManager : SingletonMono<UIManager>
    {

        #region Components

        private Transform canvasTf;

        private Transform eventSystemTf;

        #endregion

        #region Private Variables

        private List<UIBase> uiList;

        #endregion

        #region Setup

        protected override void OnAwakeInit()
        {
            base.OnAwakeInit();

            // hierarchy�����ò㼶
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            // Canvas
            canvasTf = GameObject.Find("Canvas").transform;
            DontDestroyOnLoad(canvasTf);

            // EventSystem
            eventSystemTf = GameObject.Find("EventSystem").transform;
            DontDestroyOnLoad(eventSystemTf);


            // ��ʼ��˽�б���
            uiList = new List<UIBase>();

        }

        #endregion

        #region Public Functions - UI Operations

        /// <summary>
        /// ��ʾUI����hierarchy�в��������Resources/UI�ļ����м���
        /// </summary>
        /// <typeparam name="T">UI�ű���</typeparam>
        /// <param name="uiName">UI��(�ַ���)</param>
        /// <returns>UI�ű�</returns>
        public UIBase ShowUI<T>(string uiName) where T : UIBase
        {
            UIBase ui = Find(uiName);
            if (ui == null)
            {
                // ������û�У���Ҫ��Resource/UI�ļ����м���
                GameObject obj = Instantiate(Resources.Load("UI/" + uiName), canvasTf) as GameObject;

                // ������
                obj.name = uiName;

                // �����Ҫ�Ľű�
                ui = obj.AddComponent<T>();

                // ��ӵ�uiList��
                uiList.Add(ui);
            }
            else
            {
                // �������У�ֱ����ʾ
                ui.Show();
            }
            return ui;
        }

        /// <summary>
        /// ���ĳ��UI�Ľű�
        /// </summary>
        /// <typeparam name="T">UI�ű���</typeparam>
        /// <param name="uiName">UI��(�ַ���)</param>
        /// <returns>UI�ű�</returns>
        public T GetUI<T>(string uiName) where T : UIBase
        {
            UIBase ui = Find(uiName);
            if (ui != null)
            {
                return ui.GetComponent<T>();
            }
            return null;
        }

        /// <summary>
        /// ����UI
        /// </summary>
        /// <param name="uiName">UI��(�ַ���)</param>
        public void HideUI(string uiName)
        {
            UIBase ui = Find(uiName);
            if (ui != null)
            {
                ui.Hide();
            }
        }

        /// <summary>
        /// �ر�(����)UI
        /// </summary>
        /// <param name="uiName">UI��(�ַ���)</param>
        public void CloseUI(string uiName)
        {
            UIBase ui = Find(uiName);
            if (ui != null)
            {
                uiList.Remove(ui);
                Destroy(ui.gameObject);
            }
        }

        /// <summary>
        /// �ر�(����)����UI
        /// </summary>
        public void CloseAllUI()
        {
            for (int i = uiList.Count - 1; i >= 0; i--)
            {
                Destroy(uiList[i].gameObject);
            }
            uiList.Clear();
        }

        #endregion

        #region Private Functions

        /// <summary>
        /// ��uiList�в�ѯUI������UI�ű���δ��ѯ���򷵻�null
        /// </summary>
        /// <param name="uiName">UI��(�ַ���)</param>
        /// <returns></returns>
        private UIBase Find(string uiName)
        {
            for (int i = 0; i < uiList.Count; i++)
            {
                if (uiList[i].name == uiName)
                {
                    return uiList[i];
                }
            }
            return null;
        }

        #endregion
    }
}
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

            // hierarchy中设置层级
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            // Canvas
            canvasTf = GameObject.Find("Canvas").transform;
            DontDestroyOnLoad(canvasTf);

            // EventSystem
            eventSystemTf = GameObject.Find("EventSystem").transform;
            DontDestroyOnLoad(eventSystemTf);


            // 初始化私有变量
            uiList = new List<UIBase>();

        }

        #endregion

        #region Public Functions - UI Operations

        /// <summary>
        /// 显示UI，若hierarchy中不存在则从Resources/UI文件夹中加载
        /// </summary>
        /// <typeparam name="T">UI脚本类</typeparam>
        /// <param name="uiName">UI名(字符串)</param>
        /// <returns>UI脚本</returns>
        public UIBase ShowUI<T>(string uiName) where T : UIBase
        {
            UIBase ui = Find(uiName);
            if (ui == null)
            {
                // 场景中没有，需要从Resource/UI文件夹中加载
                GameObject obj = Instantiate(Resources.Load("UI/" + uiName), canvasTf) as GameObject;

                // 改名字
                obj.name = uiName;

                // 添加需要的脚本
                ui = obj.AddComponent<T>();

                // 添加到uiList中
                uiList.Add(ui);
            }
            else
            {
                // 场景中有，直接显示
                ui.Show();
            }
            return ui;
        }

        /// <summary>
        /// 获得某个UI的脚本
        /// </summary>
        /// <typeparam name="T">UI脚本类</typeparam>
        /// <param name="uiName">UI名(字符串)</param>
        /// <returns>UI脚本</returns>
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
        /// 隐藏UI
        /// </summary>
        /// <param name="uiName">UI名(字符串)</param>
        public void HideUI(string uiName)
        {
            UIBase ui = Find(uiName);
            if (ui != null)
            {
                ui.Hide();
            }
        }

        /// <summary>
        /// 关闭(销毁)UI
        /// </summary>
        /// <param name="uiName">UI名(字符串)</param>
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
        /// 关闭(销毁)所有UI
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
        /// 从uiList中查询UI，返回UI脚本，未查询到则返回null
        /// </summary>
        /// <param name="uiName">UI名(字符串)</param>
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
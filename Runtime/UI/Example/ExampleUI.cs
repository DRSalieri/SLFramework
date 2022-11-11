using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLFramework.UI;
using UnityEngine.EventSystems;

namespace SLFramework.Test
{
    public class ExampleUI : UIBase
    {
        private void Awake()
        {
            // 在Awake中注册所有UI的回调函数
            Register("StartBtn").onClick = StartBtn_onClick;
        }

        #region Callback Functions

        private void StartBtn_onClick(GameObject obj, PointerEventData pData)
        {
            Close();
        }

        #endregion
    }
}
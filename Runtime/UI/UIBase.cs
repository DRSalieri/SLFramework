using SLFramework.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SLFramework.UI
{
    public class UIBase : MonoBehaviour
    {
        /// <summary>
        /// 获取对应对象的UIEventTrigger
        /// </summary>
        /// <param name="objName">目标相对于该脚本对象的相对路径</param>
        /// <returns></returns>
        public UIEventTrigger Register(string objName)
        {
            Transform tf = transform.Find(objName);
            return UIEventTrigger.Get(tf.gameObject);
        }


        /// <summary>
        /// 显示
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 关闭(销毁)
        /// </summary>
        public virtual void Close()
        {
            UIManager.Instance.CloseUI(gameObject.name);
        }

    }
}
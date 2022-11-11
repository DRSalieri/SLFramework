using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace SLFramework.UI
{

    /// <summary>
    /// 事件监听，与UI脚本挂在同一个GameObject上
    /// </summary>
    /// 接口IPointerClickHandler表示，如果触发了OnPointerClick事件（在同一对象按下和释放指针时），则会调用该类的OnPointerClick函数
    public class UIEventTrigger : MonoBehaviour, IPointerClickHandler
    {
        public Action<GameObject, PointerEventData> onClick;

        /// <summary>
        /// 获取obj上的UIEventTrigger脚本
        /// </summary>
        /// <param name="obj">事件监听的对象</param>
        /// <returns>UIEventTrigger脚本</returns>
        public static UIEventTrigger Get(GameObject obj)
        {
            UIEventTrigger trigger = obj.GetComponent<UIEventTrigger>();
            if (trigger == null)
            {
                trigger = obj.AddComponent<UIEventTrigger>();
            }
            return trigger;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
            {
                onClick(gameObject, eventData);
            }
        }
    }

}
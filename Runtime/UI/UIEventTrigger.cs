using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace SLFramework.UI
{

    /// <summary>
    /// �¼���������UI�ű�����ͬһ��GameObject��
    /// </summary>
    /// �ӿ�IPointerClickHandler��ʾ�����������OnPointerClick�¼�����ͬһ�����º��ͷ�ָ��ʱ���������ø����OnPointerClick����
    public class UIEventTrigger : MonoBehaviour, IPointerClickHandler
    {
        public Action<GameObject, PointerEventData> onClick;

        /// <summary>
        /// ��ȡobj�ϵ�UIEventTrigger�ű�
        /// </summary>
        /// <param name="obj">�¼������Ķ���</param>
        /// <returns>UIEventTrigger�ű�</returns>
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
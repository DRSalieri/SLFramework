using SLFramework.Manager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SLFramework.UI
{
    public class UIBase : MonoBehaviour
    {
        /// <summary>
        /// ��ȡ��Ӧ�����UIEventTrigger
        /// </summary>
        /// <param name="objName">Ŀ������ڸýű���������·��</param>
        /// <returns></returns>
        public UIEventTrigger Register(string objName)
        {
            Transform tf = transform.Find(objName);
            return UIEventTrigger.Get(tf.gameObject);
        }


        /// <summary>
        /// ��ʾ
        /// </summary>
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }
        /// <summary>
        /// ����
        /// </summary>
        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        /// �ر�(����)
        /// </summary>
        public virtual void Close()
        {
            UIManager.Instance.CloseUI(gameObject.name);
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLFramework.Utils
{
    public static class UtilsClass
    {
        /// <summary>
        /// �����->���λ�õ�������XZƽ��Ľ���
        /// </summary>
        /// <param name="yPlane"></param>
        /// <param name="v"></param>
        /// <returns>�Ƿ�ɹ���⽻��</returns>
        public static bool GetMousePosition_XZ(out Vector3 v, float yPlane = 0)
        {
            // ������������Ļ�ռ�λ�õ�һ������
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 dir = r.direction;
            
            // ƽ����XZƽ�棬��û�н���
            if (dir.y.Equals(0))
            {
                v = Vector3.zero;
                return false;
            }

            // ����t
            float t = (yPlane - r.origin.y) / dir.y;
            v = r.origin + r.direction * t;
            return true;
        }

        /// <summary>
        /// �����->���λ�õ�������YZƽ��Ľ���
        /// </summary>
        /// <param name="yPlane"></param>
        /// <param name="v"></param>
        /// <returns>�Ƿ�ɹ���⽻��</returns>
        public static bool GetMousePosition_YZ(out Vector3 v, float xPlane = 0)
        {
            // ������������Ļ�ռ�λ�õ�һ������
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 dir = r.direction;

            // ƽ����XZƽ�棬��û�н���
            if (dir.x.Equals(0))
            {
                v = Vector3.zero;
                return false;
            }

            // ����t
            float t = (xPlane - r.origin.x) / dir.x;
            v = r.origin + r.direction * t;
            return true;
        }
    }
}
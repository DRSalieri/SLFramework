using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLFramework.Utils
{
    public static class UtilsClass
    {
        /// <summary>
        /// 摄像机->鼠标位置的射线与XZ平面的交点
        /// </summary>
        /// <param name="yPlane"></param>
        /// <param name="v"></param>
        /// <returns>是否成功求解交点</returns>
        public static bool GetMousePosition_XZ(out Vector3 v, float yPlane = 0)
        {
            // 摄像机到鼠标屏幕空间位置的一条射线
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 dir = r.direction;
            
            // 平行于XZ平面，则没有交点
            if (dir.y.Equals(0))
            {
                v = Vector3.zero;
                return false;
            }

            // 计算t
            float t = (yPlane - r.origin.y) / dir.y;
            v = r.origin + r.direction * t;
            return true;
        }

        /// <summary>
        /// 摄像机->鼠标位置的射线与YZ平面的交点
        /// </summary>
        /// <param name="yPlane"></param>
        /// <param name="v"></param>
        /// <returns>是否成功求解交点</returns>
        public static bool GetMousePosition_YZ(out Vector3 v, float xPlane = 0)
        {
            // 摄像机到鼠标屏幕空间位置的一条射线
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 dir = r.direction;

            // 平行于XZ平面，则没有交点
            if (dir.x.Equals(0))
            {
                v = Vector3.zero;
                return false;
            }

            // 计算t
            float t = (xPlane - r.origin.x) / dir.x;
            v = r.origin + r.direction * t;
            return true;
        }
    }
}
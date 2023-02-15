using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLFramework.MyDebug.Runtime
{
    /// <summary>
    /// 工具类，包含各种Runtime时调用的Debug函数
    /// </summary>
    public static class RuntimeDebug
    {
        /// <summary>
        /// 新建一个文本GameObject(TextMesh)
        /// </summary>
        /// <param name="parent">父亲</param>
        /// <param name="text">文字</param>
        /// <param name="localPosition">局部坐标</param>
        /// <param name="fontSize">字号</param>
        /// <param name="color">颜色</param>
        /// <param name="textAnchor">锚点</param>
        /// <returns>TextMesh对象</returns>
        /// 
        
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor)
        {
            // 新建TextMesh类型的GameObject
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));

            // 获取该GameObject的Transform组件
            Transform transform = gameObject.transform;

            // 获取该GameObject的TextMesh组件
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();

            // 设置GameObject的父亲、局部坐标
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;

            // 设置textMesh各种属性
            textMesh.anchor = textAnchor;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.text = text;

            return textMesh;
        }
    }
}

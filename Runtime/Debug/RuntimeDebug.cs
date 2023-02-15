using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLFramework.MyDebug.Runtime
{
    /// <summary>
    /// �����࣬��������Runtimeʱ���õ�Debug����
    /// </summary>
    public static class RuntimeDebug
    {
        /// <summary>
        /// �½�һ���ı�GameObject(TextMesh)
        /// </summary>
        /// <param name="parent">����</param>
        /// <param name="text">����</param>
        /// <param name="localPosition">�ֲ�����</param>
        /// <param name="fontSize">�ֺ�</param>
        /// <param name="color">��ɫ</param>
        /// <param name="textAnchor">ê��</param>
        /// <returns>TextMesh����</returns>
        /// 
        
        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor)
        {
            // �½�TextMesh���͵�GameObject
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));

            // ��ȡ��GameObject��Transform���
            Transform transform = gameObject.transform;

            // ��ȡ��GameObject��TextMesh���
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();

            // ����GameObject�ĸ��ס��ֲ�����
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;

            // ����textMesh��������
            textMesh.anchor = textAnchor;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.text = text;

            return textMesh;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLFramework.MyDebug.Runtime;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

namespace SLFramework.Gameplay.Grid
{
    /// <summary>
    /// ������
    /// </summary>
    public class Grid_Rect<TGridObject>
    {
        #region Private Members

        /// <summary>
        /// �����X���꣩
        /// </summary>
        private int width;

        /// <summary>
        /// ����ߣ�Y/Z���꣩
        /// </summary>
        private int height;

        /// <summary>
        /// ÿ������ı߳�
        /// </summary>
        private float cellSize;

        /// <summary>
        /// ���������½ǵ���������
        /// </summary>
        private Vector3 originPosition;

        /// <summary>
        /// ��������
        /// </summary>
        private TGridObject[,] gridArray;

        #endregion

        #region Setup

        /// <summary>
        /// ����һ��������
        /// </summary>
        /// <param name="_width">��X���꣩</param>
        /// <param name="_height">�ߣ�Y/Z���꣩</param>
        /// <param name="_cellSize">���ӱ߳�</param>
        /// <param name="_originPosition">���½ǵ���������</param>
        public Grid_Rect(int _width, int _height, float _cellSize, Vector3 _originPosition)
        {
            this.width = _width;
            this.height = _height;
            this.cellSize = _cellSize;
            this.originPosition = _originPosition;

            gridArray = new TGridObject[_width, _height];
        }

        #endregion

        #region Private Methods

        private Vector3 GetWorldPosition_LD(int x, int y)
        {
            return new Vector3(x, y) * cellSize;
        }

        private Vector3 GetWorldPosition_C(int x, int y)
        {
            return new Vector3(x + 0.5f, y + 0.5f) * cellSize;
        }

        private void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x / cellSize);
            y = Mathf.FloorToInt(worldPosition.y / cellSize);
        }

        private bool IsCoordValid(int x, int y)
        {
            return (x >= 0) && (y >= 0) && (x < width) && (y < height);
        }

        #endregion

        #region Public Methods

        public void SetValue(int x,int y,TGridObject value)
        {
            if (IsCoordValid(x, y))
            {
                gridArray[x, y] = value;
            }
        }

        public void SetValue(Vector3 worldPosition, TGridObject value)
        {
            GetXY(worldPosition, out int x, out int y);
            SetValue(x, y, value);
        }

        public TGridObject GetValue(int x, int y)
        {
            if(IsCoordValid(x, y))
            {
                return gridArray[x, y];
            }
            return default(TGridObject);    // ������Чʱ����һ��Ĭ��ֵ
        }

        public TGridObject GetValue(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetValue(x, y);
        }

        #endregion

        #region Debug Methods

        /// <summary>
        /// Debug�ã�����������ͨ��Gizmo��worldText��
        /// </summary>
        /// <param name="DrawText">�Ƿ���Ƹ�������</param>
        public void Draw(bool DrawText)
        {
            // ������WorldTextͳһ����
            Transform textRoot = GameObject.Find("GridWorldTextRoot")?.transform;
            if(textRoot == null)
            {
                textRoot = new GameObject("GridWorldTextRoot").transform;
            }

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    if(DrawText)
                    {
                        RuntimeDebug.CreateWorldText(textRoot, gridArray[x, y].ToString(), GetWorldPosition_LD(x, y) + new Vector3(cellSize, cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter);
                    }

                    Debug.DrawLine(GetWorldPosition_LD(x, y), GetWorldPosition_LD(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition_LD(x, y), GetWorldPosition_LD(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition_LD(0, height), GetWorldPosition_LD(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition_LD(width, 0), GetWorldPosition_LD(width, height), Color.white, 100f);
        }

        #endregion
    }
}

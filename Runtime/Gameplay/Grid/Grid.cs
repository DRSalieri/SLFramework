using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLFramework.MyDebug.Runtime;
using System;

namespace SLFramework.Gameplay.Grid
{
    /// <summary>
    /// ������
    /// </summary>
    public class Grid<TGridObject>
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
        /// <param name="createGridObject">GridObject�ĳ�ʼ������������Ϊ(Grid<T>, int, int)������ֵΪT</param>
        public Grid(int _width, int _height, float _cellSize, Vector3 _originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
        {
            this.width = _width;
            this.height = _height;
            this.cellSize = _cellSize;
            this.originPosition = _originPosition;

            gridArray = new TGridObject[_width, _height];

            // ��ʼ��GridObject
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    gridArray[x, y] = createGridObject(this, x, y);
                }
            }
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

        #endregion

        #region Public Methods

        public void SetGridObject(int x,int y,TGridObject value)
        {
            if (IsCoordValid(x, y))
            {
                gridArray[x, y] = value;
            }
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            GetXY(worldPosition, out int x, out int y);
            SetGridObject(x, y, value);
        }

        public TGridObject GetGridObject(int x, int y)
        {
            if(IsCoordValid(x, y))
            {
                return gridArray[x, y];
            }
            return default(TGridObject);    // ������Чʱ����һ��Ĭ��ֵ
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetGridObject(x, y);
        }

        public void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            x = Mathf.FloorToInt(worldPosition.x / cellSize);
            y = Mathf.FloorToInt(worldPosition.y / cellSize);
        }

        public bool IsCoordValid(int x, int y)
        {
            return (x >= 0) && (y >= 0) && (x < width) && (y < height);
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
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

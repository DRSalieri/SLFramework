using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLFramework.Gameplay.Grid;
using System;
using SLFramework.MyDebug.Runtime;

namespace SLFramework.Gameplay.Grid
{
    public class GridXZ<TGridObject>
    {

        #region Private Members

        /// <summary>
        /// �����X���꣩
        /// </summary>
        private int width;

        /// <summary>
        /// ����ߣ�Z���꣩
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
        public GridXZ(int _width, int _height, float _cellSize, Vector3 _originPosition, Func<GridXZ<TGridObject>, int, int, TGridObject> createGridObject)
        {
            this.width = _width;
            this.height = _height;
            this.cellSize = _cellSize;
            this.originPosition = _originPosition;

            gridArray = new TGridObject[_width, _height];

            // ��ʼ��GridObject
            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    gridArray[x, z] = createGridObject(this, x, z);
                }
            }
        }

        #endregion

        #region Private Methods

        private Vector3 GetWorldPosition_LD(int x, int z)
        {
            return new Vector3(x, 0, z) * cellSize;
        }

        private Vector3 GetWorldPosition_C(int x, int z)
        {
            return new Vector3(x + 0.5f, 0, z + 0.5f) * cellSize;
        }

        #endregion

        #region Public Methods

        public void SetGridObject(int x, int z, TGridObject value)
        {
            if (IsCoordValid(x, z))
            {
                gridArray[x, z] = value;
            }
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject value)
        {
            GetXY(worldPosition, out int x, out int z);
            SetGridObject(x, z, value);
        }

        public TGridObject GetGridObject(int x, int z)
        {
            if (IsCoordValid(x, z))
            {
                return gridArray[x, z];
            }
            return default(TGridObject);    // ������Чʱ����һ��Ĭ��ֵ
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int z);
            return GetGridObject(x, z);
        }

        public void GetXY(Vector3 worldPosition, out int x, out int z)
        {
            x = Mathf.FloorToInt(worldPosition.x / cellSize);
            z = Mathf.FloorToInt(worldPosition.z / cellSize);
        }

        public bool IsCoordValid(int x, int z)
        {
            return (x >= 0) && (z >= 0) && (x < width) && (z < height);
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
            if (textRoot == null)
            {
                textRoot = new GameObject("GridWorldTextRoot").transform;
            }

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int z = 0; z < gridArray.GetLength(1); z++)
                {
                    if (DrawText)
                    {
                        RuntimeDebug.CreateWorldText(textRoot, gridArray[x, z].ToString(), GetWorldPosition_LD(x, z) + new Vector3(cellSize, 0, cellSize) * 0.5f, 20, Color.white, TextAnchor.MiddleCenter);
                    }

                    Debug.DrawLine(GetWorldPosition_LD(x, z), GetWorldPosition_LD(x, z + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition_LD(x, z), GetWorldPosition_LD(x + 1, z), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition_LD(0, height), GetWorldPosition_LD(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition_LD(width, 0), GetWorldPosition_LD(width, height), Color.white, 100f);
        }

        #endregion

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SLFramework.MyDebug.Runtime;
using System;

namespace SLFramework.Gameplay.Grid
{
    /// <summary>
    /// 网格类
    /// </summary>
    public class Grid<TGridObject>
    {
        #region Private Members

        /// <summary>
        /// 网格宽（X坐标）
        /// </summary>
        private int width;

        /// <summary>
        /// 网格高（Y/Z坐标）
        /// </summary>
        private int height;

        /// <summary>
        /// 每个网格的边长
        /// </summary>
        private float cellSize;

        /// <summary>
        /// 网格最左下角的世界坐标
        /// </summary>
        private Vector3 originPosition;

        /// <summary>
        /// 网格数据
        /// </summary>
        private TGridObject[,] gridArray;

        #endregion

        #region Setup

        /// <summary>
        /// 创建一个新网格
        /// </summary>
        /// <param name="_width">宽（X坐标）</param>
        /// <param name="_height">高（Y/Z坐标）</param>
        /// <param name="_cellSize">格子边长</param>
        /// <param name="_originPosition">左下角的世界坐标</param>
        /// <param name="createGridObject">GridObject的初始化函数，参数为(Grid<T>, int, int)，返回值为T</param>
        public Grid(int _width, int _height, float _cellSize, Vector3 _originPosition, Func<Grid<TGridObject>, int, int, TGridObject> createGridObject)
        {
            this.width = _width;
            this.height = _height;
            this.cellSize = _cellSize;
            this.originPosition = _originPosition;

            gridArray = new TGridObject[_width, _height];

            // 初始化GridObject
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
            return default(TGridObject);    // 坐标无效时返回一个默认值
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
        /// Debug用，画出该网格（通过Gizmo与worldText）
        /// </summary>
        /// <param name="DrawText">是否绘制格内内容</param>
        public void Draw(bool DrawText)
        {
            // 把所有WorldText统一管理
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

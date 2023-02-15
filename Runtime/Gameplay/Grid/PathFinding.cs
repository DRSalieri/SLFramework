using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLFramework.Gameplay.Grid
{

    /// <summary>
    /// 寻路用网格的GridObject
    /// </summary>
    public class PathNode
    {
        #region Private Members

        /// <summary>
        /// 属于的grid
        /// </summary>
        private Grid<PathNode> grid;

        #endregion

        #region Public Members

        /// <summary>
        /// x坐标
        /// </summary>
        public int x;

        /// <summary>
        /// y坐标
        /// </summary>
        public int y;

        /// <summary>
        /// 起点A到该方格的实际移动代价
        /// </summary>
        public int gCost;

        /// <summary>
        /// 该方格到终点的预计代价（计算方法不固定，使用启发函数来计算）
        /// </summary>
        public int hCost;

        /// <summary>
        /// F = G + H
        /// </summary>
        public int fCost;

        /// <summary>
        /// 是否可以行走
        /// </summary>
        public bool isWalkable;

        /// <summary>
        /// 指针，指向来源网格（寻路结束后反推得出路径）
        /// </summary>
        public PathNode cameFromNode;

        #endregion

        #region Setup

        public PathNode(Grid<PathNode> _grid, int _x, int _y)
        {
            this.grid = _grid;
            this.x = _x;
            this.y = _y;
            this.isWalkable = true;
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return x + "," + y;
        }

        /// <summary>
        /// 通过G与H计算F（F = G + H）
        /// </summary>
        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        #endregion
    }

    public class PathFinding
    {
        #region Const Members

        /// <summary>
        /// 直向移动代价
        /// </summary>
        private const int MOVE_STRAIGHT_COST = 10;

        /// <summary>
        /// 斜向移动代价
        /// </summary>
        private const int MOVE_DIAGONAL_COST = 14;

        #endregion

        #region Private Members

        /// <summary>
        /// 寻路用网格
        /// </summary>
        private Grid<PathNode> grid;

        /// <summary>
        /// 所有被考虑用于寻找最短路的格子
        /// </summary>
        private List<PathNode> openList;

        /// <summary>
        /// 不会再被考虑的格子
        /// </summary>
        private List<PathNode> closedList;

        #endregion

        #region Public Members

        /// <summary>
        /// 是否可以斜向移动
        /// </summary>
        public bool canDiagonal;

        #endregion

        #region Setup

        /// <summary>
        /// 新建一个PathFinding及其网格
        /// </summary>
        /// <param name="_width">网格宽</param>
        /// <param name="_height">网格高</param>
        public PathFinding(int _width, int _height, float _cellSize, Vector3 _originPosition, bool _canDiagonal = false)
        {
            grid = new Grid<PathNode>(_width, _height, _cellSize, _originPosition, 
                (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));         // 传入一个Lambda函数，表示PathNode的初始化方法

            canDiagonal = _canDiagonal;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 计算a到b的预计代价（最小代价）
        /// 一般用于求解fCost
        /// </summary>
        /// <param name="a">起点</param>
        /// <param name="b">终点</param>
        /// <returns>预计代价</returns>
        private int CalculateDistanceCost(PathNode a, PathNode b)
        {

            if (canDiagonal == true)
            {
                int xDistance = Mathf.Abs(a.x - b.x);
                int yDistance = Mathf.Abs(a.y - b.y);
                int remaining = Mathf.Abs(xDistance - yDistance);
                return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
            }
            else
            {
                int xDistance = Mathf.Abs(a.x - b.x);
                int yDistance = Mathf.Abs(a.y - b.y);
                return (xDistance + yDistance) * MOVE_STRAIGHT_COST;
            }
        }

        /// <summary>
        /// 返回某个list中FCost最小的节点
        /// </summary>
        /// <param name="pathNodeList">list</param>
        /// <returns>该list中FCost最小的节点</returns>
        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            PathNode lowerFCostNode = pathNodeList[0];
            for (int i = 1; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].fCost < lowerFCostNode.fCost)
                {
                    lowerFCostNode = pathNodeList[i];
                }
            }
            return lowerFCostNode;
        }

        /// <summary>
        /// 寻路结束后，导出路径（靠cameFromNode寻找）
        /// </summary>
        /// <param name="endNode"></param>
        /// <returns></returns>
        private List<PathNode> CalculatePath(PathNode endNode)
        {
            List<PathNode> path = new List<PathNode>();
            path.Add(endNode);
            PathNode currentNode = endNode;
            while(currentNode.cameFromNode != null)
            {
                path.Add(currentNode.cameFromNode);
                currentNode = currentNode.cameFromNode;
            }
            path.Reverse();

            return path;
        }

        /// <summary>
        /// 获取某PathNode所有有效邻居节点
        /// </summary>
        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            // 可以斜向移动时
            if (canDiagonal == true)
            {

                if (currentNode.x - 1 >= 0)
                {
                    // L
                    neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
                    // LD
                    if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
                    // LU
                    if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
                }
                if (currentNode.x + 1 < grid.GetWidth())
                {
                    // R
                    neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
                    // RD
                    if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
                    // RU
                    if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
                }
                // D
                if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
                // U
                if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

                return neighbourList;
            }
            // 不可以斜向移动时
            else
            {
                // L
                if (currentNode.x - 1 >= 0)
                    neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
                // R
                if (currentNode.x + 1 < grid.GetWidth())
                    neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
                // D
                if (currentNode.y - 1 >= 0) 
                    neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
                // U
                if (currentNode.y + 1 < grid.GetHeight()) 
                    neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

                return neighbourList;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 执行寻路，返回PathNode的列表
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        public List<PathNode> FindPath_Node(int startX, int startY, int endX, int endY)
        {
            PathNode startNode = grid.GetGridObject(startX, startY);
            PathNode endNode = grid.GetGridObject(endX, endY);

            openList = new List<PathNode> { startNode };        // openList初始化，仅有起始节点
            closedList = new List<PathNode>();                  // closedList初始化，空

            // PathNode初始化
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    PathNode pathNode = grid.GetGridObject(x, y);
                    pathNode.gCost = int.MaxValue;              // gCost初始化为inf
                    pathNode.hCost = 0;                         // hCost初始化为0
                    pathNode.CalculateFCost();                  // fCost初始化为inf
                    pathNode.cameFromNode = null;               // 指针设为空
                }
            }

            // 初始化startNode的G、H、F
            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                // 选出openList中FCost最小的一个
                PathNode currentNode = GetLowestFCostNode(openList);

                // 跳出条件
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                // 将该节点从openList移动入closedList中
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // 松弛currentNode的所有邻居节点
                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    // closedList中的结点不考虑
                    if (closedList.Contains(neighbourNode)) continue;

                    // 不可走的结点不考虑
                    if (neighbourNode.isWalkable == false)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    // 计算startNode -> ... -> currentNode -> neighbourNode的GCost
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);

                    // startNode到neighbouNode可以通过currentNode优化
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;

                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        // 若可以优化，将neighbourNode加入openList
                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }

            // 起点、终点不连通
            return null;

        }

        /// <summary>
        /// 执行寻路，返回二维坐标的列表
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        public List<Vector2> FindPath_Vec(int startX, int startY, int endX, int endY)
        {
            List<PathNode> pathNodes = FindPath_Node(startX, startY, endX, endY);

            // 没有路径则直接跳出
            if (pathNodes == null)
                return null;

            List<Vector2> ret = new List<Vector2>();

            foreach(PathNode p in pathNodes)
            {
                ret.Add(new Vector2(p.x, p.y));
            }

            return ret;
        }

        /// <summary>
        /// 获取(x,y)的PathNode
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PathNode GetNode(int x, int y)
        {
            return grid.GetGridObject(x, y);
        }

        /// <summary>
        /// 获取寻路用的Grid
        /// </summary>
        /// <returns></returns>
        public Grid<PathNode> GetGrid()
        {
            return grid;
        }

        /// <summary>
        /// 设置某格的可行性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="_isWalkable"></param>
        public void SetWalkable(int x, int y, bool _isWalkable)
        {
            grid.GetGridObject(x, y).isWalkable = _isWalkable;
        }

        /// <summary>
        /// 获取某格的可行性
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool GetWalkable(int x, int y)
        {
            return grid.GetGridObject(x, y).isWalkable;
        }

        #endregion

        #region Debug

        /// <summary>
        /// Debug用，画出Grid
        /// </summary>
        public void Draw(bool DrawText)
        {
            grid.Draw(DrawText);
        }

        #endregion
    }

}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace SLFramework.Gameplay.Grid
{

    /// <summary>
    /// Ѱ·�������GridObject
    /// </summary>
    public class PathNode
    {
        #region Private Members

        /// <summary>
        /// ���ڵ�grid
        /// </summary>
        private Grid<PathNode> grid;

        #endregion

        #region Public Members

        /// <summary>
        /// x����
        /// </summary>
        public int x;

        /// <summary>
        /// y����
        /// </summary>
        public int y;

        /// <summary>
        /// ���A���÷����ʵ���ƶ�����
        /// </summary>
        public int gCost;

        /// <summary>
        /// �÷����յ��Ԥ�ƴ��ۣ����㷽�����̶���ʹ���������������㣩
        /// </summary>
        public int hCost;

        /// <summary>
        /// F = G + H
        /// </summary>
        public int fCost;

        /// <summary>
        /// �Ƿ��������
        /// </summary>
        public bool isWalkable;

        /// <summary>
        /// ָ�룬ָ����Դ����Ѱ·�������Ƶó�·����
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
        /// ͨ��G��H����F��F = G + H��
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
        /// ֱ���ƶ�����
        /// </summary>
        private const int MOVE_STRAIGHT_COST = 10;

        /// <summary>
        /// б���ƶ�����
        /// </summary>
        private const int MOVE_DIAGONAL_COST = 14;

        #endregion

        #region Private Members

        /// <summary>
        /// Ѱ·������
        /// </summary>
        private Grid<PathNode> grid;

        /// <summary>
        /// ���б���������Ѱ�����·�ĸ���
        /// </summary>
        private List<PathNode> openList;

        /// <summary>
        /// �����ٱ����ǵĸ���
        /// </summary>
        private List<PathNode> closedList;

        #endregion

        #region Public Members

        /// <summary>
        /// �Ƿ����б���ƶ�
        /// </summary>
        public bool canDiagonal;

        #endregion

        #region Setup

        /// <summary>
        /// �½�һ��PathFinding��������
        /// </summary>
        /// <param name="_width">�����</param>
        /// <param name="_height">�����</param>
        public PathFinding(int _width, int _height, float _cellSize, Vector3 _originPosition, bool _canDiagonal = false)
        {
            grid = new Grid<PathNode>(_width, _height, _cellSize, _originPosition, 
                (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));         // ����һ��Lambda��������ʾPathNode�ĳ�ʼ������

            canDiagonal = _canDiagonal;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// ����a��b��Ԥ�ƴ��ۣ���С���ۣ�
        /// һ���������fCost
        /// </summary>
        /// <param name="a">���</param>
        /// <param name="b">�յ�</param>
        /// <returns>Ԥ�ƴ���</returns>
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
        /// ����ĳ��list��FCost��С�Ľڵ�
        /// </summary>
        /// <param name="pathNodeList">list</param>
        /// <returns>��list��FCost��С�Ľڵ�</returns>
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
        /// Ѱ·�����󣬵���·������cameFromNodeѰ�ң�
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
        /// ��ȡĳPathNode������Ч�ھӽڵ�
        /// </summary>
        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            // ����б���ƶ�ʱ
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
            // ������б���ƶ�ʱ
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
        /// ִ��Ѱ·������PathNode���б�
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

            openList = new List<PathNode> { startNode };        // openList��ʼ����������ʼ�ڵ�
            closedList = new List<PathNode>();                  // closedList��ʼ������

            // PathNode��ʼ��
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    PathNode pathNode = grid.GetGridObject(x, y);
                    pathNode.gCost = int.MaxValue;              // gCost��ʼ��Ϊinf
                    pathNode.hCost = 0;                         // hCost��ʼ��Ϊ0
                    pathNode.CalculateFCost();                  // fCost��ʼ��Ϊinf
                    pathNode.cameFromNode = null;               // ָ����Ϊ��
                }
            }

            // ��ʼ��startNode��G��H��F
            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();

            while (openList.Count > 0)
            {
                // ѡ��openList��FCost��С��һ��
                PathNode currentNode = GetLowestFCostNode(openList);

                // ��������
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                // ���ýڵ��openList�ƶ���closedList��
                openList.Remove(currentNode);
                closedList.Add(currentNode);

                // �ɳ�currentNode�������ھӽڵ�
                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    // closedList�еĽ�㲻����
                    if (closedList.Contains(neighbourNode)) continue;

                    // �����ߵĽ�㲻����
                    if (neighbourNode.isWalkable == false)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }

                    // ����startNode -> ... -> currentNode -> neighbourNode��GCost
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);

                    // startNode��neighbouNode����ͨ��currentNode�Ż�
                    if (tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;

                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();

                        // �������Ż�����neighbourNode����openList
                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }

            // ��㡢�յ㲻��ͨ
            return null;

        }

        /// <summary>
        /// ִ��Ѱ·�����ض�ά������б�
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="endX"></param>
        /// <param name="endY"></param>
        /// <returns></returns>
        public List<Vector2> FindPath_Vec(int startX, int startY, int endX, int endY)
        {
            List<PathNode> pathNodes = FindPath_Node(startX, startY, endX, endY);

            // û��·����ֱ������
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
        /// ��ȡ(x,y)��PathNode
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public PathNode GetNode(int x, int y)
        {
            return grid.GetGridObject(x, y);
        }

        /// <summary>
        /// ��ȡѰ·�õ�Grid
        /// </summary>
        /// <returns></returns>
        public Grid<PathNode> GetGrid()
        {
            return grid;
        }

        /// <summary>
        /// ����ĳ��Ŀ�����
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="_isWalkable"></param>
        public void SetWalkable(int x, int y, bool _isWalkable)
        {
            grid.GetGridObject(x, y).isWalkable = _isWalkable;
        }

        /// <summary>
        /// ��ȡĳ��Ŀ�����
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
        /// Debug�ã�����Grid
        /// </summary>
        public void Draw(bool DrawText)
        {
            grid.Draw(DrawText);
        }

        #endregion
    }

}
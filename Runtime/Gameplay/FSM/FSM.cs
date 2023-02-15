using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLFramework.Gameplay.FSM
{
    /// <summary>
    /// 所有状态都需要实现该接口
    /// 记录状态的函数、所需信息等
    /// </summary>
    public interface IState
    {
        void OnEnter(int from);
        void OnUpdate();
        void OnExit(int to);

    }

    public class FSM<T> where T : IState
    {
        #region Private Members

        /// <summary>
        /// 记录该状态机的所有状态
        /// </summary>
        private readonly Dictionary<int, IState> states =
            new Dictionary<int, IState>();

        /// <summary>
        /// 记录该状态机的所有转换
        /// </summary>
        private List<(int currentState, int nextState, int eventCode)> transitions =
            new List<(int currentState, int nextState, int eventCode)> ();

        /// <summary>
        /// 能自跳跃的状态
        /// </summary>
        private HashSet<int> triggerSelfStates =
            new HashSet<int>();

        /// <summary>
        /// Update前调用的函数
        /// </summary>
        private System.Action beforeUpdateCallback;
        /// <summary>
        /// Update后调用的函数
        /// </summary>
        private System.Action afterUpdateCallback;

        #endregion

        #region Public Members

        /// <summary>
        /// 当前状态
        /// </summary>
        public int currentState { get; private set; }

        #endregion

        #region Setup

        public FSM(int defaultState = -1, System.Action beforeUpdate = null, System.Action afterUpdate = null)
        {
            currentState = defaultState;
            beforeUpdateCallback = beforeUpdate;
            afterUpdateCallback = afterUpdate;
        }


        #endregion

        #region Private Methods

        #endregion

        #region Public Methods

        /// <summary>
        /// 向状态机添加某状态、及其信息（实现过IState接口）
        /// </summary>
        /// <param name="_stateId">状态id，可由枚举类转来</param>
        /// <param name="_stateInfo">实现过IState接口</param>
        /// <returns>是否成功</returns>
        /// <exception cref="System.Exception"></exception>
        public bool AddState(int _stateId, T _stateInfo)
        {
            // 已存在该状态
            if(states.ContainsKey(_stateId))
            {
                throw new System.Exception($"不能向状态机重复添加(_stateId)行为");
            }

            // 不存在该状态
            states.Add(_stateId, _stateInfo);
            return true;
        }

        /// <summary>
        /// 向状态机添加状态跳转
        /// </summary>
        /// <param name="_from">起始状态</param>
        /// <param name="_to">目标状态</param>
        /// <param name="_triggerCode">触发事件</param>
        /// <returns>是否成功</returns>
        /// <exception cref="System.Exception"></exception>
        public bool AddTransition(int _from, int _to, int _triggerCode)
        {
            // 某状态不存在
            if(!states.ContainsKey(_from) || !states.ContainsKey(_to))
            {
                throw new System.Exception($"添加状态转换失败，状态(_from)或状态(_to)不存在");
            }

            // 自跳转
            if(_from == _to)
            {
                triggerSelfStates.Add(_from);
            }

            transitions.Add(item: (_from, _to, _triggerCode));
            return true;
        }
        
        /// <summary>
        /// 将所有状态都向某状态添加一个状态转换（默认排除自己）
        /// </summary>
        /// <param name="_to">目标状态</param>
        /// <param name="_triggerCode">触发事件</param>
        /// <param name="_exclludeSelf">是否排除自己</param>
        /// <returns>是否成功</returns>
        /// <exception cref="System.Exception"></exception>
        public bool AddAnyToTransition(int _to, int _triggerCode, bool _exclludeSelf = true)
        {
            // 目标状态不存在
            if(states.ContainsKey(_to) == false)
            {
                throw new System.Exception($"添加状态转换失败，状态(_to)不存在");
            }

            foreach(int from in states.Keys)
            {
                // 自跳转时
                if(from == _to)
                {
                    if (_exclludeSelf)
                    {
                        continue;
                    }
                    else
                    {
                        triggerSelfStates.Add(_to);
                    }
                }

                transitions.Add(item: (from, _to, _triggerCode));
            }
            return true;
        }

        /// <summary>
        /// 触发某事件（实现状态转换）
        /// 每次触发最多触发一次状态转换
        /// </summary>
        /// <param name="_eventCode">触发事件</param>
        /// <returns>是否发生状态转换</returns>
        public bool TriggerEvent(int _eventCode)
        {
            // 遍历所有状态转换
            foreach(var transition in transitions)
            {
                if(transition.currentState == currentState && transition.eventCode == _eventCode)
                {
                    SwitchToState(transition.nextState);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 删除某状态
        /// </summary>
        /// <param name="_stateID">待删除状态</param>
        /// <returns>是否成功删除</returns>
        /// <exception cref="System.Exception"></exception>
        public bool RemoveState(int _stateID)
        {
            // 若不包含该状态
            if(states.ContainsKey(_stateID) == false)
            {
                throw new System.Exception($"删除状态失败，状态(_stateID)不存在");
            }

            states.Remove(_stateID);
            return true;
        }

        /// <summary>
        /// 转移到某状态
        /// </summary>
        /// <param name="_stateID">目标状态</param>
        /// <param name="forceSwitch">本状态跳本状态时，若不存在自跳转，是否触发OnExit和OnEnter</param>
        /// <returns>是否调用了OnEnter和OnExit函数</returns>
        public bool SwitchToState(int _stateID, bool forceSwitch = false)
        {
            // if(states.ContainsKey(_stateID))

            
            if (_stateID == currentState)
            {
                // 本状态跳本状态，若不存在自跳转且不强制执行，直接退出
                if (!forceSwitch && !triggerSelfStates.Contains(_stateID))
                {
                    return false;
                }
            }

            // 调用原状态OnExit函数
            if (states.TryGetValue(currentState, out var oldStateInfo))
            {
                oldStateInfo.OnExit(_stateID);
            }

            // 切换状态
            int oldStateId = currentState;
            currentState = _stateID;

            // 调用新状态OnEnter函数
            if(states.TryGetValue(_stateID, out var newStateInfo))
            {
                newStateInfo.OnEnter(oldStateId);
            }

            return true;
        }

        /// <summary>
        /// Update函数
        /// </summary>
        public void Update()
        {
            if(states.TryGetValue(currentState, out var stateInfo))
            {
                beforeUpdateCallback?.Invoke();
                stateInfo.OnUpdate();
                afterUpdateCallback?.Invoke();
            }
        }

        #endregion
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLFramework.Gameplay.FSM
{
    /// <summary>
    /// ����״̬����Ҫʵ�ָýӿ�
    /// ��¼״̬�ĺ�����������Ϣ��
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
        /// ��¼��״̬��������״̬
        /// </summary>
        private readonly Dictionary<int, IState> states =
            new Dictionary<int, IState>();

        /// <summary>
        /// ��¼��״̬��������ת��
        /// </summary>
        private List<(int currentState, int nextState, int eventCode)> transitions =
            new List<(int currentState, int nextState, int eventCode)> ();

        /// <summary>
        /// ������Ծ��״̬
        /// </summary>
        private HashSet<int> triggerSelfStates =
            new HashSet<int>();

        /// <summary>
        /// Updateǰ���õĺ���
        /// </summary>
        private System.Action beforeUpdateCallback;
        /// <summary>
        /// Update����õĺ���
        /// </summary>
        private System.Action afterUpdateCallback;

        #endregion

        #region Public Members

        /// <summary>
        /// ��ǰ״̬
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
        /// ��״̬�����ĳ״̬��������Ϣ��ʵ�ֹ�IState�ӿڣ�
        /// </summary>
        /// <param name="_stateId">״̬id������ö����ת��</param>
        /// <param name="_stateInfo">ʵ�ֹ�IState�ӿ�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        /// <exception cref="System.Exception"></exception>
        public bool AddState(int _stateId, T _stateInfo)
        {
            // �Ѵ��ڸ�״̬
            if(states.ContainsKey(_stateId))
            {
                throw new System.Exception($"������״̬���ظ����(_stateId)��Ϊ");
            }

            // �����ڸ�״̬
            states.Add(_stateId, _stateInfo);
            return true;
        }

        /// <summary>
        /// ��״̬�����״̬��ת
        /// </summary>
        /// <param name="_from">��ʼ״̬</param>
        /// <param name="_to">Ŀ��״̬</param>
        /// <param name="_triggerCode">�����¼�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        /// <exception cref="System.Exception"></exception>
        public bool AddTransition(int _from, int _to, int _triggerCode)
        {
            // ĳ״̬������
            if(!states.ContainsKey(_from) || !states.ContainsKey(_to))
            {
                throw new System.Exception($"���״̬ת��ʧ�ܣ�״̬(_from)��״̬(_to)������");
            }

            // ����ת
            if(_from == _to)
            {
                triggerSelfStates.Add(_from);
            }

            transitions.Add(item: (_from, _to, _triggerCode));
            return true;
        }
        
        /// <summary>
        /// ������״̬����ĳ״̬���һ��״̬ת����Ĭ���ų��Լ���
        /// </summary>
        /// <param name="_to">Ŀ��״̬</param>
        /// <param name="_triggerCode">�����¼�</param>
        /// <param name="_exclludeSelf">�Ƿ��ų��Լ�</param>
        /// <returns>�Ƿ�ɹ�</returns>
        /// <exception cref="System.Exception"></exception>
        public bool AddAnyToTransition(int _to, int _triggerCode, bool _exclludeSelf = true)
        {
            // Ŀ��״̬������
            if(states.ContainsKey(_to) == false)
            {
                throw new System.Exception($"���״̬ת��ʧ�ܣ�״̬(_to)������");
            }

            foreach(int from in states.Keys)
            {
                // ����תʱ
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
        /// ����ĳ�¼���ʵ��״̬ת����
        /// ÿ�δ�����ഥ��һ��״̬ת��
        /// </summary>
        /// <param name="_eventCode">�����¼�</param>
        /// <returns>�Ƿ���״̬ת��</returns>
        public bool TriggerEvent(int _eventCode)
        {
            // ��������״̬ת��
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
        /// ɾ��ĳ״̬
        /// </summary>
        /// <param name="_stateID">��ɾ��״̬</param>
        /// <returns>�Ƿ�ɹ�ɾ��</returns>
        /// <exception cref="System.Exception"></exception>
        public bool RemoveState(int _stateID)
        {
            // ����������״̬
            if(states.ContainsKey(_stateID) == false)
            {
                throw new System.Exception($"ɾ��״̬ʧ�ܣ�״̬(_stateID)������");
            }

            states.Remove(_stateID);
            return true;
        }

        /// <summary>
        /// ת�Ƶ�ĳ״̬
        /// </summary>
        /// <param name="_stateID">Ŀ��״̬</param>
        /// <param name="forceSwitch">��״̬����״̬ʱ��������������ת���Ƿ񴥷�OnExit��OnEnter</param>
        /// <returns>�Ƿ������OnEnter��OnExit����</returns>
        public bool SwitchToState(int _stateID, bool forceSwitch = false)
        {
            // if(states.ContainsKey(_stateID))

            
            if (_stateID == currentState)
            {
                // ��״̬����״̬��������������ת�Ҳ�ǿ��ִ�У�ֱ���˳�
                if (!forceSwitch && !triggerSelfStates.Contains(_stateID))
                {
                    return false;
                }
            }

            // ����ԭ״̬OnExit����
            if (states.TryGetValue(currentState, out var oldStateInfo))
            {
                oldStateInfo.OnExit(_stateID);
            }

            // �л�״̬
            int oldStateId = currentState;
            currentState = _stateID;

            // ������״̬OnEnter����
            if(states.TryGetValue(_stateID, out var newStateInfo))
            {
                newStateInfo.OnEnter(oldStateId);
            }

            return true;
        }

        /// <summary>
        /// Update����
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

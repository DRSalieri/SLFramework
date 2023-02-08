using System;
using UnityEngine;

namespace SLFramework 
{
    
    public class MonoController : MonoBehaviour
    {
        private event Action UpdateAction;

        // Awakeʱ������ΪDontDestroyOnLoad
        private void Awake() => DontDestroyOnLoad(gameObject);

        // Updateʱ�������е�UpdateAction
        private void Update() => UpdateAction?.Invoke();

        public void AddEventListener(Action action) => UpdateAction += action;

        public void RemoveEventListener(Action action) => UpdateAction -= action;
    }
}

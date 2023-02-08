using System;
using UnityEngine;

namespace SLFramework 
{
    
    public class MonoController : MonoBehaviour
    {
        private event Action UpdateAction;

        // Awake时设置其为DontDestroyOnLoad
        private void Awake() => DontDestroyOnLoad(gameObject);

        // Update时调用所有的UpdateAction
        private void Update() => UpdateAction?.Invoke();

        public void AddEventListener(Action action) => UpdateAction += action;

        public void RemoveEventListener(Action action) => UpdateAction -= action;
    }
}

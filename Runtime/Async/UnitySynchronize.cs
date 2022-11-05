using System.Threading;
using UnityEngine;

namespace SLFramework.Async
{
    public static class UnitySynchronize
    {
        public static SynchronizationContext synchronizationContext;

        /// <summary>
        /// 载入新场景时自动切换线程上下文
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            synchronizationContext = SynchronizationContext.Current;
        }
    }

    /// <summary>
    /// 自定义的yield指令
    /// yield return WaitForUpdate 等价于 yield return null
    /// 但为了GetAwaiter，单独写出来
    /// </summary>
    public class WaitForUpdate : CustomYieldInstruction
    {
        public override bool keepWaiting => false;
    }
}
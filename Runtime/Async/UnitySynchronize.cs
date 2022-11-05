using System.Threading;
using UnityEngine;

namespace SLFramework.Async
{
    public static class UnitySynchronize
    {
        public static SynchronizationContext synchronizationContext;

        /// <summary>
        /// �����³���ʱ�Զ��л��߳�������
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Install()
        {
            synchronizationContext = SynchronizationContext.Current;
        }
    }

    /// <summary>
    /// �Զ����yieldָ��
    /// yield return WaitForUpdate �ȼ��� yield return null
    /// ��Ϊ��GetAwaiter������д����
    /// </summary>
    public class WaitForUpdate : CustomYieldInstruction
    {
        public override bool keepWaiting => false;
    }
}
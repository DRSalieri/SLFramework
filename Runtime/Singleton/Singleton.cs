
namespace SLFramework.Singleton
{
    /// <summary>
    /// ��������
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : new()   // new()��ʾ����Ҫ��һ���޲ι��캯��
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new T();
                }
                return instance;
            }
        }
    }
}
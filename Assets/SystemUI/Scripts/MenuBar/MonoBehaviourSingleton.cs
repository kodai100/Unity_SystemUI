using UnityEngine;

namespace inc.stu.SystemUI.MenuBar
{
    public class MonoBehaviourSingleton<T> : MonoBehaviour where T : MonoBehaviourSingleton<T>
    {
        private static T _instance;
        public static T Instance => _instance;
        public static bool IsValid => _instance != null;

        public static void Release()
        {
            if (IsValid)
            {
                Destroy(_instance);
            }
            _instance.Deinit();
            _instance = null;
        }

        private void Awake()
        {
            if (!IsValid)
            {
                _instance = this as T;
                if(_instance) _instance.Init();
            }
            else
            {
                Debug.Log("すでにSingletonのインスタンスが存在しています");
            }
        }

        private void OnDestroy()
        {
            _instance.Deinit();
        }

        protected virtual void Init()
        {

        }

        protected virtual void Deinit()
        {

        }
    }

}


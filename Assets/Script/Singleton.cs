using UnityEngine;

namespace Script
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var gameObject = new GameObject();
                    gameObject.name = typeof(T).Name;
                    gameObject.hideFlags = HideFlags.HideAndDontSave;
                    _instance = gameObject.AddComponent<T>();
                }

                return _instance;
            }
        }
    }
}
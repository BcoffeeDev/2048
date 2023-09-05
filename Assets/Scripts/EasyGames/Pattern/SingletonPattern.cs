using UnityEngine;

namespace EasyGames.Pattern
{
    public class SingletonPattern<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                var obj = Object.FindObjectsOfType<T>();
                if (obj.Length > 0)
                    _instance = obj[0];
                if (obj.Length > 1)
                    Debug.LogError($"Instance of {typeof(T)} is more than 1!");
                if (obj.Length <= 0)
                {
                    var newObj = new GameObject();
                    _instance = newObj.AddComponent<T>();
                }

                return _instance;
            }
        }
    }
}

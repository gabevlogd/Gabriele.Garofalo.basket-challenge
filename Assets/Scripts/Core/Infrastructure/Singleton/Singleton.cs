using UnityEngine;

namespace BasketChallenge.Core
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance => _instance;
        private static T _instance;

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                if (!TryGetComponent<T>(out _instance))
                {
                    _instance = gameObject.AddComponent<T>();
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}

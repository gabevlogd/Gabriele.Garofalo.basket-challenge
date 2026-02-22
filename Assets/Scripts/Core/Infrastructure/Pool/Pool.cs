using System.Collections.Generic;
using UnityEngine;

namespace BasketChallenge.Core
{
    /// <summary>
    /// Pool pattern that manages generic type TPoolObj of type Component
    /// </summary>
    public abstract class Pool<TPoolObj> : MonoBehaviour where TPoolObj : Component
    {
        [SerializeField] protected TPoolObj poolObjPrefab;
        private List<TPoolObj> _available;

        /// <summary>
        /// Initializes the pool by instantiating an initial number of TPoolObj
        /// </summary>
        /// <param name="startCount">Initial number of TPoolObj</param>
        protected void InitializePool(int startCount)
        {
            if (_available != null)
            {
                Debug.LogWarning($"{typeof(TPoolObj)} Pool already initialized");
                return;
            }

            _available = new List<TPoolObj>();

            for (int i = 0; i < startCount; i++)
            {
                TPoolObj obj = Instantiate(poolObjPrefab, transform);
                _available.Add(obj);
            }
        }

        /// <summary>
        /// Get a TPoolObj from the pool if there are available
        /// </summary>
        /// <param name="forced">If true instantiates a new TPoolObj if there aren't available and return the new one</param>
        /// <returns></returns>
        protected TPoolObj GetObject(bool forced = false)
        {
            TPoolObj result;

            if (_available.Count != 0)
            {
                result = _available[^1];
                _available.RemoveAt(_available.Count - 1);
            }
            else if (forced)
                result = Instantiate(poolObjPrefab, transform);
            else
            {
                Debug.LogWarning($"No available {typeof(TPoolObj)}, return null ref");
                return null;
            }

            result.gameObject.SetActive(true);
            return result;
        }

        /// <summary>
        /// Release a TPoolObj no longer needed
        /// </summary>
        /// <param name="poolObj"></param>
        protected void ReleaseObject(TPoolObj poolObj)
        {
            poolObj.gameObject.SetActive(false);
            _available.Add(poolObj);
        }
    }
}

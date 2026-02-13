using System;
using UnityEngine;

namespace BasketChallenge.Core
{
    public class SkinnedMeshComponent : MonoBehaviour
    {
        [SerializeField]
        protected GameObject _skinnedMesh;
        
        [SerializeField]
        protected Animator _animator;
        
        /// <summary>
        /// Tries to find a socket (a child GameObject) by name under the SkinnedMesh. This is useful for attaching items,
        /// effects, or other GameObjects to specific points on the character's mesh, such as hands, head, or feet.
        /// If the socket is not found, it logs a warning and returns null.
        /// </summary>
        /// <param name="socketName"></param>
        /// <returns></returns>
        public GameObject TryGetSocket(string socketName)
        {
            Transform socketTransform = _skinnedMesh.transform.Find(socketName);
            if (socketTransform == null)
            {
                Debug.LogWarning($"Socket '{socketName}' not found in SkinnedMeshComponent.");
                return null;
            }
            return socketTransform.gameObject;
        }
        
        /// <summary>
        /// Tries to find a socket's Transform by name under the SkinnedMesh.
        /// This is a convenience method that calls TryGetSocket and returns the Transform directly.
        /// If the socket is not found, it logs a warning and returns null
        /// </summary>
        /// <param name="socketName"></param>
        /// <returns></returns>
        public Transform TryGetSocketTransform(string socketName)
        {
            GameObject go = TryGetSocket(socketName);
            return go != null ? go.transform : null;
        }

        
    }
}

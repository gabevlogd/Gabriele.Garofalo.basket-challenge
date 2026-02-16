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
        /// <param name="socketTransform"></param>
        /// <returns></returns>
        public bool TryGetSocketTransform(string socketName, out Transform socketTransform)
        {
            socketTransform = _skinnedMesh.transform.Find(socketName);
            if (!socketTransform)
            {
                Debug.LogWarning($"Socket '{socketName}' not found in SkinnedMeshComponent.");
                return false;
            }
            return true;
        }

        
    }
}

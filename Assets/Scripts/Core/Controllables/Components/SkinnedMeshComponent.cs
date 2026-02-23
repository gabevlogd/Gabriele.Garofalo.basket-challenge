using UnityEngine;

namespace BasketChallenge.Core
{
    public class SkinnedMeshComponent : MonoBehaviour
    {
        [SerializeField] protected GameObject skinnedMesh;
        public GameObject SkinnedMesh => skinnedMesh;
        
        [SerializeField] protected Animator animator;
        public Animator Animator => animator;
        
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
            socketTransform = skinnedMesh.transform.Find(socketName);
            if (!socketTransform)
            {
                Debug.LogWarning($"Socket '{socketName}' not found in SkinnedMeshComponent.");
                return false;
            }
            return true;
        }

        
    }
}

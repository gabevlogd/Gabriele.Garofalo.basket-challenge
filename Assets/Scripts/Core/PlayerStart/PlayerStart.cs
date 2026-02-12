using UnityEngine;

namespace BasketChallenge.Core
{
    [ExecuteInEditMode]
    public class PlayerStart : MonoBehaviour
    {
        [SerializeField] float radius = 0.35f;
        [SerializeField] float forwardLength = 1.25f;

        private void OnDrawGizmos()
        {
            if (Application.isPlaying) return;
            
            Gizmos.color = Color.green;
            
            var p = transform.position;
            var f = transform.forward;

            Gizmos.DrawWireSphere(p, radius);
            
            Gizmos.color = Color.blue;

            Gizmos.DrawLine(p, p + f * forwardLength);

            Vector3 right = Quaternion.AngleAxis(25f, transform.up) * (-f);
            Vector3 left  = Quaternion.AngleAxis(-25f, transform.up) * (-f);
            
            Gizmos.DrawLine(p + f * forwardLength, p + f * forwardLength + right * 0.35f);
            Gizmos.DrawLine(p + f * forwardLength, p + f * forwardLength + left * 0.35f);
        }
    }
}

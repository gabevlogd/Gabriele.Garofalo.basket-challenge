using BasketChallenge.Core;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(BoxCollider))]
    public class RimHitDetector : MonoBehaviour
    {
        private void Awake()
        {
            if (!TryGetComponent(out BoxCollider boxCollider))
            {
                Debug.LogError("RimHitDetector requires a BoxCollider component to function properly.");
            }
            else
            {
                boxCollider.isTrigger = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out BasketBall ball))
            {
                if (ball.LastThrowOutcome is ThrowOutcome.FarRim or ThrowOutcome.NearRim)
                {
                    SoundManager.Play("RimHit", false);
                }
            }
        }
    }
}

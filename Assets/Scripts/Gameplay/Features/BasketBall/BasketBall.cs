using System;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
    public class BasketBall : MonoBehaviour
    {
        public Rigidbody Rigidbody => _rigidbody;
        private Rigidbody _rigidbody;

        protected void Awake()
        {
            if (!TryGetComponent(out _rigidbody))
            {
                Debug.LogError("BasketBall requires a Rigidbody component to function properly.");
            }
        }
    }
}
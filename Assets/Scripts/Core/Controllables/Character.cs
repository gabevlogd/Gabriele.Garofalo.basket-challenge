
using System;
using UnityEngine;

namespace BasketChallenge.Core
{
    [RequireComponent(typeof(SkinnedMeshComponent))]
    public class Character : ControllableBase
    {
        public SkinnedMeshComponent SkinnedMeshComponent { get; private set; }

        private void Awake()
        {
            if (!TryGetComponent(out SkinnedMeshComponent SkinnedMeshComponent))
            {
                Debug.LogError("Character requires a SkinnedMeshComponent, but none was found on the GameObject.");
            }
        }
    }
}
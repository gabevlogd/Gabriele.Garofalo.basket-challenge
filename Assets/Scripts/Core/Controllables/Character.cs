
using System;
using UnityEngine;

namespace BasketChallenge.Core
{
    [RequireComponent(typeof(SkinnedMeshComponent))]
    public class Character : ControllableBase
    {
        [HideInInspector]
        public SkinnedMeshComponent skinnedMeshComponent;

        protected virtual void Awake()
        {
            if (!TryGetComponent(out skinnedMeshComponent))
            {
                Debug.LogError("Character requires a SkinnedMeshComponent, but none was found on the GameObject.");
            }
        }
    }
}
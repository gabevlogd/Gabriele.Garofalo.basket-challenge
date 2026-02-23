using System;
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    public class CharacterAnimEvents : MonoBehaviour
    {
        public event Action OnThrowAnimationEvent;
        public event Action OnThrowCompleteAnimationEvent;
        
        private void Throw()
        {
            OnThrowAnimationEvent?.Invoke();
        }

        private void ThrowComplete()
        {
            OnThrowCompleteAnimationEvent?.Invoke();
        }
    }
}

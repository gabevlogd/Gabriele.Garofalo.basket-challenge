using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "AIBehaviourData", menuName = "ScriptableObjects/AIBehaviourData", order = 0)]
    public class AIBehaviourData : ScriptableObject
    {
        [Header("Throw Timing")]
        [Min(0.1f), SerializeField] private float throwDelayMin;
        [Min(0.1f), SerializeField] private float throwDelayMax;
        
        [Header("Throw Outcome Weights")]
        public float perfectThrowWeight = 1f;
        public float backboardThrowWeight = 1f;
        public float nearRimThrowWeight = 1f;
        public float farRimThrowWeight = 1f;
        public float shortMissThrowWeight = 1f;
        public float longMissThrowWeight = 1f;
        public float backboardMissThrowWeight = 1f;
        
        public float ThrowDelayMin => Mathf.Min(throwDelayMin, throwDelayMax);
        public float ThrowDelayMax => Mathf.Max(throwDelayMin, throwDelayMax);
    }
}
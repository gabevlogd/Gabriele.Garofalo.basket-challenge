using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "FireBallData", menuName = "ScriptableObjects/FireBallData", order = 0)]
    public class FireBallData : ScriptableObject
    {
        public float fireDuration;
        
        [Range(0f,1f)] public float fireAmountPerScore;
        
        public float fireAmountDecraseSpeed;
    }
}
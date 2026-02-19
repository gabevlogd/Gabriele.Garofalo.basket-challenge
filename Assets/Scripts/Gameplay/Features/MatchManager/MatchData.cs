using UnityEngine;
using UnityEngine.Serialization;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "MatchData", menuName = "MatchData", order = 0)]
    public class MatchData : ScriptableObject
    {
        public float startDelay;
        public float duration;
        public float extraTimeDuration;
        public float endDuration;
    }
}
using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [CreateAssetMenu(fileName = "PowerRangesContainer", menuName = "ScriptableObjects/PowerRangesContainer", order = 0)]
    public class PowerRangesContainer : ScriptableObject
    {
        public ThrowPowerRanges ranges;
    }
}
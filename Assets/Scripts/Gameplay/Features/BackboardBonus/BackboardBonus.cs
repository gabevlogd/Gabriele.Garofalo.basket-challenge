using UnityEngine;

namespace BasketChallenge.Gameplay
{
    [System.Serializable]
    public class BackboardBonus
    {
        public int extraPoints;

        public Color color = Color.white;

        [Range(0, 1)] public float triggerChance;
        
        public bool TryTriggerBonus()
        {
            return Random.value <= triggerChance;
        }
    }
}